
-- USER
select * from docs.mt_doc_user 

select 
	id,
 	data ->> 'FirstName' as FirstName,
	data ->> 'LastName' as LastName,
	data ->> 'Gender' as Gender,
	data,
	data - 'Gender' #- '{Address,City}'  as Light
from docs.mt_doc_user 



-- ISSUE
select * from docs.mt_doc_issue

select 
	id,
	data ->> 'Name' as Name,
	data ->> 'Tags' as Tags,
	data ->> 'AssigneeId' as AssigneeId
from docs.mt_doc_issue



-- SMURF
select * from docs.mt_doc_smurf 

select 
	id,
 	data ->> 'Ability' as Ability,
 	data ->> 'Rank' as Rank 	
from docs.mt_doc_smurf



-- TEXT SEARCH
select * from docs.mt_doc_textsearch 

select 
	(data ->> 'Text') as text,
	(data ->> 'Text') <-> 'air fran' as d
from docs.mt_doc_textsearch 
order by (data ->> 'Text') <-> 'air fran'

-- indexing
CREATE INDEX mt_doc_textsearch_idx_text ON docs.mt_doc_textsearch USING gist ((data->>'Text') gist_trgm_ops)

explain
select *
from docs.mt_doc_textsearch 
order by data ->> 'Text' <-> 'air fran'
limit 100

truncate docs.mt_doc_textsearch 



-- MAPPING SIMPLE
select * from docs.mt_doc_mappingsimple

select * from docs.mt_doc_mappingsimple where id = 'french id 5'

select * from docs.mt_doc_mappingsimple where data ->> 'FrenchId' = 'french id 5'



-- MAPPING COMPLEX
select * from docs.mt_doc_mappingcomplex

select * from docs.mt_doc_mappingcomplex where data -> 'FrenchIds' ? 'french id 50000';

select * from docs.mt_doc_mappingcomplex where data @> '{"FrenchIds":["french id 50000"]}';


CREATE INDEX mt_doc_mappingcomplex_idx_french_ids ON docs.mt_doc_mappingcomplex USING gin (  (CAST(data ->> 'FrenchIds' as jsonb)));

CREATE INDEX mt_doc_mappingcomplex_idx_french_ids2 ON docs.mt_doc_mappingcomplex USING gin (data jsonb_path_ops)

CREATE INDEX mt_doc_mappingcomplex_idx_french_ids ON docs.mt_doc_mappingcomplex USING gin ((data->'FrenchIds'));


-- MAPPING comparison
truncate docs.mt_doc_mappingcomplex;
truncate docs.mt_doc_mappingsimple

explain select * from docs.mt_doc_mappingsimple where id = 'french id 5';
explain select * from docs.mt_doc_mappingsimple where data ->> 'FrenchId' = 'french id 5'
-- cost without index = 7448
-- cost with    index = 8


explain select * from docs.mt_doc_mappingcomplex where data->'FrenchIds' ? 'french id 50000'
-- cost without index = 3892
-- cost with    index = 350

explain select * from docs.mt_doc_mappingcomplex where data @> '{"FrenchIds":["french id 50000"]}' -- used by marten
-- cost without index = 3642
-- cost with    index = 350


explain select * from docs.mt_doc_mappingcomplex where data->'FrenchIds' ?| array['french id 50000']
-- cost without index = 3892
-- cost with    index = 350

explain select * from docs.mt_doc_mappingcomplex where CAST(data->>'FrenchIds' as jsonb) ?| array['french id 50000'] -- used by marten
-- cost without index = 4393
-- cost with    index = 351





-- Jsonb to row
select 
	id,
	(jsonb_each_text(data)).*
from docs.mt_doc_user 



-- Pretty
select 
	id,
	jsonb_pretty(data)
from docs.mt_doc_user 

select 
	id,
	jsonb_pretty(data)
from docs.mt_doc_issue 

select 
	id,
	jsonb_pretty(data)
from docs.mt_doc_smurf



-- Tests
delete from docs.mt_doc_issue;
delete from docs.mt_doc_user;
delete from docs.mt_doc_smurf

