# Document Store

This project is a sample for document store database, using:
- Postgresql
- [Marten](https://github.com/JasperFx/marten)

it shows:
- Sub class (`Issue`)
- Sub array (`Assignee`)
- Inheritance (`Smurf`)
- Indexing sub array (`Mapping`)
- Indexing text (`TextSearch`)


***


#### Postgresql
- [Jsonb functions](https://www.postgresql.org/docs/9.5/functions-json.html)
- [sub-array filter](https://stackoverflow.com/questions/22736742/query-for-array-elements-inside-json-type)
- [Index types](https://www.citusdata.com/blog/2017/10/17/tour-of-postgres-index-types/)
  - [gin index](https://www.postgresql.org/docs/9.4/datatype-json.html#JSON-INDEXING)
  - [full text search index](https://www.postgresql.org/docs/9.5/textsearch-tables.html#TEXTSEARCH-TABLES-INDEX)
  - [similar text index](https://www.postgresql.org/docs/9.4/pgtrgm.html#AEN163078) ([functions](https://www.postgresql.org/docs/current/functions-matching.html#FUNCTIONS-POSIX-REGEXP))
- [index usage](https://medium.com/@Alibaba_Cloud/principles-and-optimization-of-5-postgresql-indexes-btree-hash-gin-gist-and-brin-4d133e7f1842)

> need `pg_trgm` extension


#### Marten
- [Docs](http://jasperfx.github.io/marten/documentation/)
- [Query](http://jasperfx.github.io/marten/documentation/documents/querying/linq/)
- [Include](http://jasperfx.github.io/marten/documentation/documents/querying/include/)
- [Inheritance](http://jasperfx.github.io/marten/documentation/documents/advanced/hierarchies/)

##### Id generation
- [marten identity](https://jasperfx.github.io/marten/documentation/documents/identity/)
- [hi-lo algorithm](https://stackoverflow.com/questions/282099/whats-the-hi-lo-algorithm)
- `int/long` vs `uuid/guid`
  - [stackoverflow](https://dba.stackexchange.com/questions/264/guid-vs-int-which-is-better-as-a-primary-key)
  - [usage](https://tomharrisonjr.com/uuid-or-guid-as-primary-keys-be-careful-7b2aa3dcb439) => Integers Internal, UUIDs External

#### Migrations
[Marten migration](http://jasperfx.github.io/marten/documentation/schema/migrations/)

```sql
-- rename column
update example
set js = js - 'old' || jsonb_build_object('new', js->'old')
where js ? 'old'

-- transform array<T> to T
update example
set js = jsonb_set(
            js #- '{prefixPath,old}',
            '{prefixPath,new}',
            js #> '{prefixPath,old,0}') -- take only first item
where js ? '{prefixPath,old}'
```


#### Note
- [`Include` with JoinType.LeftOuter and dictionary fails](https://github.com/JasperFx/marten/pull/1223)
- [IsOneOf for array using include fails](https://github.com/JasperFx/marten/pull/1221)
- [Containment where clauses](https://github.com/JasperFx/marten/issues/1345)
