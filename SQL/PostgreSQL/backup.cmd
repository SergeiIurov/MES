 @echo off

for /f "usebackq tokens=1" %%a in (`WMIC OS GET LocalDateTime /value ^| find "LocalDateTime="`) do set DTS=%%a
set "current_date=%DTS:~14,4%-%DTS:~18,2%-%DTS:~20,2%"

    REM Set your database details
    set DB_NAME=MES
    set PG_USER=postgres
    set PG_HOST=localhost
    set PG_PORT=5432
    set BACKUP_DIR=e:\backups\
    set PGPASSWORD=1

    REM Construct the backup file name
    set BACKUP_FILE=%BACKUP_DIR%\%DB_NAME%_%current_date%.sql

    REM Execute pg_dump
    echo Backing up %DB_NAME% to %BACKUP_FILE%...
    C:\"Program Files"\PostgreSQL\17\bin\pg_dump -h %PG_HOST% -p %PG_PORT% -U %PG_USER% -F p -d %DB_NAME% > "%BACKUP_FILE%"

    echo Backup complete %BACKUP_FILE%!
	
	set DB_NAME=AUTH
	
	REM Construct the backup file name
    set BACKUP_FILE=%BACKUP_DIR%\%DB_NAME%_%current_date%.sql

	REM Execute pg_dump
    echo Backing up %DB_NAME% to %BACKUP_FILE%...
    C:\"Program Files"\PostgreSQL\17\bin\pg_dump -h %PG_HOST% -p %PG_PORT% -U %PG_USER% -F p -d %DB_NAME% > "%BACKUP_FILE%"

    echo Backup complete %BACKUP_FILE%!