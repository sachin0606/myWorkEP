@echo off
title EPG / QAM Ops Console
cd /d "%~dp0"
powershell -NoProfile -ExecutionPolicy Bypass -File "%~dp0EPG-Ops.ps1" %*
if errorlevel 1 pause
