#!/bin/sh
echo "Waiting for PostgreSQL..."

until nc -z medicore_postgres 5432
do
  echo "Postgres not ready - sleeping"
  sleep 2
done

echo "PostgreSQL is ready!"
exec "$@"

