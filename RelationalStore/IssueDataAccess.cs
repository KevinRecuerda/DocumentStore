namespace RelationalStore
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Threading.Tasks;
    using Dapper;
    using Dapper.FastCrud;
    using Model;

    public class IssueDataAccess : BaseDataAccess
    {
        public IssueDataAccess(IConnectionStrings connectionStrings) : base(connectionStrings)
        {
            var issueMap = OrmConfiguration.GetDefaultEntityMapping<IssueDto>();
            if (!issueMap.IsFrozen)
                issueMap.SetSchemaName("rel")
                          .SetTableName("issues")
                          .SetProperty(issue => issue.Id, prop => prop.SetPrimaryKey().SetDatabaseGenerated(DatabaseGeneratedOption.Identity));

            var issueTagMap = OrmConfiguration.GetDefaultEntityMapping<IssueTagDto>();
            if (!issueTagMap.IsFrozen)
                issueTagMap.SetSchemaName("rel")
                           .SetTableName("issue_tags");
        }

        public async Task<IEnumerable<Issue>> GetAll()
        {
            const string query = @"
select * from rel.issues;
select * from rel.issue_tags";

            using (var db = this.Open())
            using (var multi = await db.QueryMultipleAsync(query))
            {
                var issues = multi.Read<Issue>().ToList();
                var tagDtos = multi.Read<IssueTagDto>().ToList();

                var tagsByIssueId = tagDtos.GroupBy(t => t.IssueId).ToDictionary(g => g.Key);
                foreach (var issue in issues)
                {
                    var tags = tagsByIssueId.ContainsKey(issue.Id)
                                   ? tagsByIssueId[issue.Id].Select(t => t.Name).ToList()
                                   : new List<string>();
                    issue.Tags = tags;
                }

                return issues;
            }
        }

        public async Task<IEnumerable<Issue>> GetByTags(params string[] tags)
        {
            var query = $@"
create temp table ids (id BIGINT);

insert into ids
select distinct i.id
from rel.issues i
left join rel.issue_tags t
    on t.issue_id = i.id
where t.name = ANY (:{nameof(tags)});

select i.* 
from rel.issues i
join ids
    on i.id = ids.id;

select t.*
from rel.issue_tags t
join ids
    on t.issue_id = ids.id;

drop table ids;";

            using (var db = this.Open())
            using (var multi = await db.QueryMultipleAsync(query, new {tags }))
            {
                var issues  = multi.Read<Issue>().ToList();
                var tagDtos = multi.Read<IssueTagDto>().ToList();

                var tagsByIssueId = tagDtos.GroupBy(t => t.IssueId).ToDictionary(g => g.Key);
                foreach (var issue in issues)
                {
                    var tag = tagsByIssueId.ContainsKey(issue.Id)
                                   ? tagsByIssueId[issue.Id].Select(t => t.Name).OrderBy(t => t).ToList()
                                   : new List<string>();
                    issue.Tags = tag;
                }

                return issues;
            }
        }

        public async Task<Issue> GetById(long id)
        {
            var query = $@"
select * from rel.issues where id = :{nameof(id)};
select * from rel.issue_tags where issue_id = :{nameof(id)}";

            using (var db = this.Open())
            using (var multi = await db.QueryMultipleAsync(query, new {id}))
            {
                var issue = multi.Read<Issue>().SingleOrDefault();
                var tags   = multi.Read<IssueTagDto>().ToList();

                if (issue != null)
                    issue.Tags = tags.Select(t => t.Name).ToList();

                return issue;
            }
        }

        public async Task Insert(Issue issue)
        {
            using (var db = this.Open())
            {
                var (dto, tagDtos) = issue.ToDto();
                await db.InsertAsync(dto);

                foreach (var issueTagDto in tagDtos)
                {
                    issueTagDto.IssueId = dto.Id;
                    await db.InsertAsync(issueTagDto);
                }

                issue.Id = dto.Id;
            }
        }

        public async Task Update(Issue issue)
        {
            using (var db = this.Open())
            {
                var (dto, tagDtos) = issue.ToDto();
                await db.UpdateAsync(dto);

                await db.BulkDeleteAsync<IssueTagDto>(
                    statement => statement.Where($"issue_id = :{nameof(issue.Id)}")
                                          .WithParameters(new {issue.Id}));
                foreach (var issueTagDto in tagDtos)
                    await db.InsertAsync(issueTagDto);
            }
        }

        public async Task Delete(Issue issue)
        {
            using (var db = this.Open())
            {
                var (dto, tagDtos) = issue.ToDto();
                await db.UpdateAsync(dto);

                await db.BulkDeleteAsync<IssueTagDto>(
                    statement => statement.Where($"issue_id = :{nameof(issue.Id)}")
                                          .WithParameters(new {issue.Id}));

                await db.DeleteAsync(dto);
            }
        }
    }

    public class IssueDto
    {
        public long   Id        { get; set; }
        public string Name { get; set; }

        public long? AssigneeId { get; set; }
        public long? ReporterId { get; set; }
    }

    public class IssueTagDto
    {
        public long   IssueId      { get; set; }
        public string Name { get; set; }
    }

    public static class IssueDtoExtensions
    {
        public static (IssueDto, List<IssueTagDto>) ToDto(this Issue issue)
        {
            var dto = new IssueDto()
            {
                Id         = issue.Id,
                Name       = issue.Name,
                AssigneeId = issue.AssigneeId,
                ReporterId = issue.ReporterId
            };

            var tags = issue.Tags
                           ?.Select(
                                 t => new IssueTagDto()
                                 {
                                     IssueId = dto.Id,
                                     Name    = t
                                 })
                            .ToList()
                ?? new List<IssueTagDto>();

            return (dto, tags);
        }
    }
}