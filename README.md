# Document Store

This project is a sample for document store database, using:
- Postgresql
- [Marten](https://github.com/JasperFx/marten)

***

#### Postgresql
- [Jsonb functions](https://www.postgresql.org/docs/9.5/functions-json.html)
- [Index types](https://www.citusdata.com/blog/2017/10/17/tour-of-postgres-index-types/)
- [Index usage](https://medium.com/@Alibaba_Cloud/principles-and-optimization-of-5-postgresql-indexes-btree-hash-gin-gist-and-brin-4d133e7f1842)


#### Marten
- [Docs](http://jasperfx.github.io/marten/documentation/)
- [Query](http://jasperfx.github.io/marten/documentation/documents/querying/linq/)
- [Include](http://jasperfx.github.io/marten/documentation/documents/querying/include/)
- [Inheritance](http://jasperfx.github.io/marten/documentation/documents/advanced/hierarchies/)


#### Migrations
[Marten migration](http://jasperfx.github.io/marten/documentation/schema/migrations/)

```sql
update example
set js = js - 'nme' || jsonb_build_object('name', js->'nme')
where js ? 'nme'
```