create schema admin AUTHORIZATION admin_user

GRANT USAGE ON SCHEMA admin TO admin_user;
GRANT SELECT, INSERT ON ALL TABLES IN SCHEMA admin TO admin_user;

SELECT schema_name FROM information_schema.schemata;

SELECT * FROM hr.employees

ALTER TABLE hr.employees
ADD age int null