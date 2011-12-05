::-----------------------------
::** Initialize
@ECHO OFF
SETLOCAL ENABLEEXTENSIONS
REM Initialize Constants
SET TSVN_INFO_FILE=.\TSVN_INFO.tmp
REM Initialize script arguments
SET workDir=%1
SET template=%2
SET target=%3

REM Goto main entry
GOTO MAIN
::=============================


::-----------------------------
::** Main entry
:MAIN
pushd %workDir%
SET workDir=.\

REM ������
IF %workDir%=="" GOTO ARGUMENT_ERROR
IF %template%=="" GOTO ARGUMENT_ERROR
IF %target%=="" GOTO ARGUMENT_ERROR

REM ��ѯע���
reg query HKLM\SOFTWARE\TortoiseSVN /v Directory > %TSVN_INFO_FILE% 2>NUL

REM ���� TSVN ·��
FOR /F "tokens=*" %%i IN (%TSVN_INFO_FILE%) DO (
  ECHO %%i | find "Directory    REG_SZ" > NUL
  IF %ERRORLEVEL% == 0 (
    ECHO %%i > %TSVN_INFO_FILE%
  )
)
SET /P TSVN_PATH= < %TSVN_INFO_FILE%
SET TSVN_PATH=%TSVN_PATH:~23,-1%

REM ���� TSVN �滻ģ��
IF NOT %ERRORLEVEL% == 0 GOTO UNKNOW_ERROR
"%TSVN_PATH%bin\SubWCRev.exe" %workDir% %template% %target%
IF NOT %ERRORLEVEL% == 0 GOTO UNKNOW_ERROR
GOTO SUCESSED
::=============================


::-----------------------------
::** Error handlers

:ARGUMENT_ERROR
ECHO ����Ĳ�����Ч��
GOTO FAIL

:NOT_FOUND_TSVN
ECHO ��ѯTortoiseSVN �İ�װ��Ϣʧ�ܡ�
GOTO FAIL

:UNKNOW_ERROR
ECHO δ֪����
:FAIL
::=============================

::-----------------------------
::** Program exit
:FAIL
DEL /Q %TSVN_INFO_FILE% 2>NUL
ECHO ģ���滻ʧ�ܡ�
popd
EXIT 1

:SUCESSED
DEL /Q %TSVN_INFO_FILE% 2>NUL
ECHO �ɹ�������ģ���滻��
popd
EXIT 0
::=============================
