#Requires -Version 5.1
<#
.SYNOPSIS
  Add EPG program events through the EPG 2.4 UI (FormEITManage -> Add -> Submit).

.DESCRIPTION
  Does not write SQL and does not patch EPG binaries.
  It clicks the same controls a user would, so EITInfoManager.Save() and
  validation in FormEITManageDetail still run.

  1. Start EPG2.0.exe and log in yourself.
  2. Edit events.csv (see events.sample.csv).
  3. Run this script. Keep the EPG window visible; do not use the mouse
     while it is filling forms.

.EXAMPLE
  powershell -STA -File .\Add-EpgUiEvents.ps1 -CsvPath .\events.csv

.EXAMPLE
  powershell -STA -File .\Add-EpgUiEvents.ps1 -Inspect
#>
[CmdletBinding()]
param(
    [string]$CsvPath = (Join-Path $PSScriptRoot "events.csv"),
    [string]$ProcessName = "EPG2.0",
    [int]$StepDelayMs = 400,
    [switch]$Inspect,
    [switch]$SkipOpenMenu
)

if ([Threading.Thread]::CurrentThread.GetApartmentState() -ne "STA") {
    $argList = @("-NoProfile", "-STA", "-ExecutionPolicy", "Bypass", "-File", $PSCommandPath)
    foreach ($k in $PSBoundParameters.Keys) {
        $v = $PSBoundParameters[$k]
        if ($v -is [switch]) {
            if ($v) { $argList += "-$k" }
        }
        else {
            $argList += "-$k"
            $argList += "$v"
        }
    }
    $p = Start-Process -FilePath "powershell.exe" -ArgumentList $argList -Wait -PassThru -NoNewWindow
    exit $p.ExitCode
}

Add-Type -AssemblyName UIAutomationClient
Add-Type -AssemblyName UIAutomationTypes
Add-Type -AssemblyName System.Windows.Forms

$UIA = [System.Windows.Automation.AutomationElement]
$CT = [System.Windows.Automation.ControlType]
$TreeScope = [System.Windows.Automation.TreeScope]
$PropertyCondition = [System.Windows.Automation.PropertyCondition]
$AndCondition = [System.Windows.Automation.AndCondition]
$OrCondition = [System.Windows.Automation.OrCondition]
$InvokePattern = [System.Windows.Automation.InvokePattern]
$ValuePattern = [System.Windows.Automation.ValuePattern]
$ExpandCollapsePattern = [System.Windows.Automation.ExpandCollapsePattern]
$SelectionItemPattern = [System.Windows.Automation.SelectionItemPattern]
$LegacyIAccessiblePattern = [System.Windows.Automation.LegacyIAccessiblePattern]

function Wait-Ms([int]$ms) { Start-Sleep -Milliseconds $ms }

function Get-EpgProcess {
    $proc = Get-Process -Name $ProcessName -ErrorAction SilentlyContinue | Select-Object -First 1
    if (-not $proc) { throw "EPG client is not running. Start '$ProcessName.exe' and log in first." }
    if ($proc.MainWindowHandle -eq [IntPtr]::Zero) { throw "EPG window handle is zero. Log in and bring the main window to the front." }
    return $proc
}

function Get-MainElement {
    $proc = Get-EpgProcess
    $el = $UIA::FromHandle($proc.MainWindowHandle)
    if (-not $el) { throw "Could not attach to EPG main window." }
    $el.SetFocus() | Out-Null
    return $el
}

function Find-By([System.Windows.Automation.AutomationElement]$root, [System.Windows.Automation.Condition]$cond, [int]$timeoutMs = 8000) {
    $sw = [Diagnostics.Stopwatch]::StartNew()
    do {
        $hit = $root.FindFirst($TreeScope::Descendants, $cond)
        if ($hit) { return $hit }
        Wait-Ms 150
    } while ($sw.ElapsedMilliseconds -lt $timeoutMs)
    return $null
}

function NameIs($text) {
    return New-Object $PropertyCondition.FullName($UIA::NameProperty, $text)
}

function TypeIs($controlType) {
    return New-Object $PropertyCondition.FullName($UIA::ControlTypeProperty, $controlType)
}

function IdIs($automationId) {
    return New-Object $PropertyCondition.FullName($UIA::AutomationIdProperty, $automationId)
}

function AnyName([string[]]$names) {
    $conds = foreach ($n in $names) { NameIs $n }
    if ($conds.Count -eq 1) { return $conds[0] }
    return New-Object $OrCondition.FullName(, [System.Windows.Automation.Condition[]]$conds)
}

function Find-Named($root, [string[]]$names, $controlType = $null, [int]$timeoutMs = 8000) {
    $nameCond = AnyName $names
    $cond = if ($controlType) { New-Object $AndCondition.FullName($nameCond, (TypeIs $controlType)) } else { $nameCond }
    return Find-By $root $cond $timeoutMs
}

function Get-Descendants($root, $controlType = $null) {
    $cond = if ($controlType) { TypeIs $controlType } else { [System.Windows.Automation.Condition]::TrueCondition }
    return @($root.FindAll($TreeScope::Descendants, $cond))
}

function Show-Tree($root, [int]$depth = 0, [int]$maxDepth = 6) {
    if ($depth -gt $maxDepth -or -not $root) { return }
    $pad = "  " * $depth
    $ct = $root.Current.ControlType.ProgrammaticName -replace "ControlType\.", ""
    Write-Host ("{0}[{1}] Name='{2}' Id='{3}'" -f $pad, $ct, $root.Current.Name, $root.Current.AutomationId)
    foreach ($c in @($root.FindAll($TreeScope::Children, [System.Windows.Automation.Condition]::TrueCondition))) {
        Show-Tree $c ($depth + 1) $maxDepth
    }
}

function Test-Pattern($el, $pattern) {
    $unused = $null
    return [bool]$el.TryGetCurrentPattern($pattern, [ref]$unused)
}

function Invoke-Element($el, [string]$what) {
    if (-not $el) { throw "Control not found: $what" }
    if (Test-Pattern $el $InvokePattern::Pattern) {
        $p = $el.GetCurrentPattern($InvokePattern::Pattern)
        $p.Invoke()
    }
    else {
        $el.SetFocus()
        [System.Windows.Forms.SendKeys]::SendWait("{ENTER}")
    }
    Wait-Ms $StepDelayMs
}

function Set-EditValue($el, [string]$text, [string]$what) {
    if (-not $el) { throw "Edit box not found: $what" }
    $el.SetFocus()
    Wait-Ms 80
    if (Test-Pattern $el $ValuePattern::Pattern) {
        $p = $el.GetCurrentPattern($ValuePattern::Pattern)
        $p.SetValue($text)
    }
    else {
        [System.Windows.Forms.SendKeys]::SendWait("^a")
        [System.Windows.Forms.SendKeys]::SendWait("{DEL}")
        if ($text) { [System.Windows.Forms.SendKeys]::SendWait($text.Replace("+", "{+}").Replace("^", "{^}").Replace("%", "{%}")) }
    }
    Wait-Ms $StepDelayMs
}

function Select-ComboItem($combo, [string]$want, [string]$what) {
    if (-not $combo) { throw "Combo box not found: $what" }
    $combo.SetFocus()
    if (Test-Pattern $combo $ExpandCollapsePattern::Pattern) {
        $combo.GetCurrentPattern($ExpandCollapsePattern::Pattern).Expand()
        Wait-Ms 250
    }
    else {
        [System.Windows.Forms.SendKeys]::SendWait("%{DOWN}")
        Wait-Ms 250
    }
    $items = Get-Descendants $combo $CT::ListItem
    if ($items.Count -eq 0) {
        $desktop = $UIA::RootElement
        $items = Get-Descendants $desktop $CT::ListItem
    }
    $hit = $items | Where-Object { $_.Current.Name -and ($_.Current.Name -eq $want -or $_.Current.Name -like "*$want*") } | Select-Object -First 1
    if (-not $hit) {
        $shown = ($items | ForEach-Object { $_.Current.Name } | Select-Object -First 20) -join ", "
        throw "No combo item matching '$want' in $what. Visible: $shown"
    }
    if (Test-Pattern $hit $SelectionItemPattern::Pattern) {
        $hit.GetCurrentPattern($SelectionItemPattern::Pattern).Select()
    }
    else {
        Invoke-Element $hit $want
    }
    Wait-Ms ($StepDelayMs + 200)
}

function Select-ListItem($list, [string]$want, [string]$what) {
    if (-not $list) { throw "List box not found: $what" }
    $items = Get-Descendants $list $CT::ListItem
    $hit = $items | Where-Object { $_.Current.Name -and ($_.Current.Name -eq $want -or $_.Current.Name -like "*$want*") } | Select-Object -First 1
    if (-not $hit) {
        $shown = ($items | ForEach-Object { $_.Current.Name } | Select-Object -First 20) -join ", "
        throw "No program matching '$want' in $what. Visible: $shown"
    }
    if (Test-Pattern $hit $SelectionItemPattern::Pattern) {
        $hit.GetCurrentPattern($SelectionItemPattern::Pattern).Select()
    }
    else {
        Invoke-Element $hit $want
    }
    Wait-Ms ($StepDelayMs + 200)
}

function Set-DateTimePickers($dialog, [datetime]$start, [datetime]$end) {
    $dtps = Get-Descendants $dialog $CT::Custom | Where-Object { $_.Current.ClassName -like "*SysDateTimePick32*" }
    if ($dtps.Count -eq 0) {
        $walker = [System.Windows.Automation.TreeWalker]::RawViewWalker
        $stack = New-Object System.Collections.Generic.Stack[object]
        $stack.Push($dialog)
        $found = New-Object System.Collections.Generic.List[object]
        while ($stack.Count -gt 0) {
            $n = $stack.Pop()
            if ($n.Current.ClassName -eq "SysDateTimePick32") { $found.Add($n) }
            $child = $walker.GetFirstChild($n)
            while ($child) {
                $stack.Push($child)
                $child = $walker.GetNextSibling($child)
            }
        }
        $dtps = @($found)
    }
    if ($dtps.Count -lt 4) {
        Write-Host "Date pickers found: $($dtps.Count). Falling back to keyboard on focused fields."
    }
    $values = @(
        $start.ToString([cultureinfo]::CurrentCulture.DateTimeFormat.ShortDatePattern),
        $start.ToString("HH:mm:ss"),
        $end.ToString([cultureinfo]::CurrentCulture.DateTimeFormat.ShortDatePattern),
        $end.ToString("HH:mm:ss")
    )
    for ($i = 0; $i -lt [Math]::Min(4, $dtps.Count); $i++) {
        $el = $dtps[$i]
        $el.SetFocus()
        Wait-Ms 80
        if (Test-Pattern $el $ValuePattern::Pattern) {
            $el.GetCurrentPattern($ValuePattern::Pattern).SetValue($values[$i])
        }
        else {
            [System.Windows.Forms.SendKeys]::SendWait("^a")
            [System.Windows.Forms.SendKeys]::SendWait($values[$i])
        }
        Wait-Ms $StepDelayMs
    }
}

function Open-EitManage($main) {
    $already = Find-Named $main @("EIT管理", "EIT Management", "EIT Manage") $null 500
    if ($already) { return }
    # Menu: 节目管理(&P) -> 节目事件管理
    [System.Windows.Forms.SendKeys]::SendWait("%p")
    Wait-Ms 400
    $item = Find-Named $UIA::RootElement @("节目事件管理", "EIT Management", "Event Management") $CT::MenuItem 3000
    if ($item) {
        Invoke-Element $item "EIT Management"
    }
    else {
        [System.Windows.Forms.SendKeys]::SendWait("{DOWN}{DOWN}{ENTER}")
        Wait-Ms $StepDelayMs
    }
    $opened = Find-Named $main @("EIT管理", "EIT Management", "EIT Manage") $null 8000
    if (-not $opened) { throw "EIT Management form did not open. Open 节目管理 -> 节目事件管理 yourself and re-run with -SkipOpenMenu." }
}

function Get-EitManage($main) {
    $form = Find-Named $main @("EIT管理", "EIT Management", "EIT Manage") $null 3000
    if (-not $form) { $form = $main }
    return $form
}

function Get-AddDialog($procId) {
    $sw = [Diagnostics.Stopwatch]::StartNew()
    do {
        $windows = Get-Descendants $UIA::RootElement $CT::Window
        foreach ($w in $windows) {
            try {
                if ($w.Current.ProcessId -ne $procId) { continue }
                $submit = Find-Named $w @("提交(S)", "提交(&S)", "Submit", "Submit(S)") $CT::Button 200
                $nameBox = Find-By $w (IdIs "txtEITName") 200
                if (-not $nameBox) { $nameBox = Find-Named $w @("事件名称：") $null 200 }
                if ($submit -and ($nameBox -or (Find-By $w (IdIs "txtEITName") 100))) {
                    return $w
                }
            }
            catch { }
        }
        Wait-Ms 150
    } while ($sw.ElapsedMilliseconds -lt 8000)
    return $null
}

function Dismiss-MessageBoxes($procId) {
    $windows = Get-Descendants $UIA::RootElement $CT::Window
    foreach ($w in $windows) {
        try {
            if ($w.Current.ProcessId -ne $procId) { continue }
            $ok = Find-Named $w @("确定", "OK") $CT::Button 200
            if ($ok -and $w.Current.Name -ne "主界面") { Invoke-Element $ok "message OK" }
        }
        catch { }
    }
}

# ---- inspect / run ----
$main = Get-MainElement
$proc = Get-EpgProcess

if ($Inspect) {
    Write-Host "Process $($proc.Id)  Title='$($main.Current.Name)'"
    Show-Tree $main 0 5
    exit 0
}

if (-not (Test-Path $CsvPath)) {
    throw "CSV not found: $CsvPath  (copy events.sample.csv to events.csv and fill your NIT / TS / program names)"
}

$rows = @(Import-Csv -Path $CsvPath)
if ($rows.Count -eq 0) { throw "CSV is empty: $CsvPath" }

if (-not $SkipOpenMenu) {
    Open-EitManage $main
    $main = Get-MainElement
}

$manage = Get-EitManage $main
$combos = @(Get-Descendants $manage $CT::ComboBox)
$lists = @(Get-Descendants $manage $CT::List)
if ($combos.Count -lt 2) { throw "Could not see NIT/TS combo boxes. Is EIT Management the active window?" }
if ($lists.Count -lt 1) { throw "Could not see the program list." }

$lastKey = ""
$okCount = 0
$fail = 0

foreach ($row in $rows) {
    $nit = "$($row.Nit)".Trim()
    $ts = "$($row.Ts)".Trim()
    $prg = "$($row.Program)".Trim()
    $name = "$($row.EventName)".Trim()
    if (-not $name) { Write-Warning "Skipping row with empty EventName"; continue }

    $start = [datetime]::Parse($row.StartTime)
    $end = [datetime]::Parse($row.EndTime)
    $key = "$nit|$ts|$prg"
    Write-Host "Adding '$name'  $start -> $end  [$key]"

    if ($key -ne $lastKey) {
        Select-ComboItem $combos[0] $nit "NIT/Network"
        $manage = Get-EitManage (Get-MainElement)
        $combos = @(Get-Descendants $manage $CT::ComboBox)
        $lists = @(Get-Descendants $manage $CT::List)
        Select-ComboItem $combos[1] $ts "TS"
        $manage = Get-EitManage (Get-MainElement)
        $lists = @(Get-Descendants $manage $CT::List)
        Select-ListItem $lists[0] $prg "Program"
        $lastKey = $key
    }

    $add = Find-Named $manage @("新增(A)", "新增(&A)", "Add", "Add(A)") $CT::Button 2000
    if (-not $add) { $add = Find-By $manage (IdIs "tsbAdd") 2000 }
    Invoke-Element $add "Add"

    $dlg = Get-AddDialog $proc.Id
    if (-not $dlg) { throw "Add Event dialog did not open. Check that a program is selected and you have Add permission." }

    $nameBox = Find-By $dlg (IdIs "txtEITName") 2000
    if (-not $nameBox) {
        $edits = @(Get-Descendants $dlg $CT::Edit)
        if ($edits.Count -ge 2) { $nameBox = $edits[1] }
    }
    Set-EditValue $nameBox $name "Event name"

    $remark = "$($row.ShortRemark)"
    $longRemark = "$($row.LongRemark)"
    $remarkBox = Find-By $dlg (IdIs "txtEITRemark") 800
    if ($remarkBox -and $remark) { Set-EditValue $remarkBox $remark "Short remark" }
    $longBox = Find-By $dlg (IdIs "txtEITLRemark") 800
    if ($longBox -and $longRemark) { Set-EditValue $longBox $longRemark "Long remark" }

    Set-DateTimePickers $dlg $start $end

    $submit = Find-Named $dlg @("提交(S)", "提交(&S)", "Submit", "Submit(S)") $CT::Button 2000
    Invoke-Element $submit "Submit"
    Wait-Ms 500
    Dismiss-MessageBoxes $proc.Id

    $stillOpen = Get-AddDialog $proc.Id
    if ($stillOpen) {
        $fail++
        Write-Warning "Add dialog still open after Submit for '$name'. Dismiss it and check validation (end after start, name required)."
        $cancel = Find-Named $stillOpen @("取消", "Cancel") $CT::Button 500
        if ($cancel) { Invoke-Element $cancel "Cancel" }
        continue
    }
    $okCount++
    $manage = Get-EitManage (Get-MainElement)
}

Write-Host "Done. Saved $okCount event(s) through the UI. Failed: $fail"
exit $(if ($fail -gt 0) { 1 } else { 0 })
