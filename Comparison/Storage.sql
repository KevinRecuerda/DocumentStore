---------------
-- FUNCTIONS
---------------

CREATE OR REPLACE FUNCTION pg_schema_size(text) RETURNS BIGINT AS $$
SELECT SUM(pg_total_relation_size(quote_ident(schemaname) || '.' || quote_ident(tablename)))::BIGINT 
FROM pg_tables 
WHERE schemaname = $1
$$ LANGUAGE SQL;


---------------
-- RESULTS
---------------

-- DATABASE
SELECT pg_size_pretty(pg_database_size(current_database()));


-- SCHEMA
select pg_size_pretty(pg_schema_size('public'));

select 
	schemaname, 
	pg_size_pretty(SUM(pg_total_relation_size(fullname)::BIGINT)) as total_size
from (
	select schemaname, quote_ident(schemaname) || '.' || quote_ident(tablename) as "fullname"
	from pg_tables 
	where schemaname NOT IN ('pg_catalog', 'information_schema')
) n
group by schemaname
order by SUM(pg_total_relation_size(fullname)::BIGINT) desc
limit 20


-- TABLE
select 
	fullname, 
	pg_size_pretty(pg_total_relation_size(fullname)::BIGINT) as total_table_size,
	pg_size_pretty(pg_relation_size(fullname)::BIGINT) as table_size,
	pg_size_pretty(pg_indexes_size(fullname)::BIGINT) as index_size
from (
	select quote_ident(schemaname) || '.' || quote_ident(tablename) as "fullname"
	from pg_tables 
	where schemaname NOT IN ('pg_catalog', 'information_schema')
) r
order by pg_total_relation_size(fullname) desc 
limit 20