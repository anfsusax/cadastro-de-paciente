#!/bin/bash
set -e

echo "Waiting for SQL Server to be ready..."
for i in {1..60}; do
    /opt/mssql-tools18/bin/sqlcmd -S sqlserver -U sa -P "Be3@Password123!" -Q "SELECT 1" -C > /dev/null 2>&1
    if [ $? -eq 0 ]; then
        echo "SQL Server is ready!"
        break
    fi
    echo "Waiting for SQL Server... ($i/60)"
    sleep 2
done

echo "Initializing database..."
/opt/mssql-tools18/bin/sqlcmd -S sqlserver -U sa -P "Be3@Password123!" -i /scripts/00_init_all.sql -C

echo "Database initialization completed!"

