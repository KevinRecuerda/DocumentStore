namespace DocumentStore.Tests
{
    using System.Linq;
    using System.Threading.Tasks;
    using DeepEqual.Syntax;
    using Model;
    using Model.Tests;
    using Xunit;

    public class MappingSimpleDataAccessTests
    {
        private readonly MappingSimpleDataAccess mappingDataAccess;

        public MappingSimpleDataAccessTests()
        {
            this.mappingDataAccess = new MappingSimpleDataAccess(DocumentStoreFactory.CreateDocumentStore(new ConnectionStrings()));
        }

        public static MappingSimple CreateMapping(string frenchId, string worldId)
        {
            return new MappingSimple
            {
                FrenchId = frenchId,
                WorldId  = worldId
            };
        }

        [Fact]
        public async Task Should_C_R_U_D()
        {
            var mapping1 = CreateMapping("-", "W1");
            var mapping2 = CreateMapping("F1", "W2");
            var mapping3 = CreateMapping("F2", "W3");
            var mapping4 = CreateMapping("F3", "W3");

            // Create
            await this.AssertInsert(mapping1);
            await this.AssertInsert(mapping2);
            await this.AssertInsert(mapping3);
            await this.AssertInsert(mapping4);

            // Read
            await this.AssertGetAll(mapping1, mapping2, mapping3, mapping4);
            await this.AssertGetByFrenchId("F1", mapping2);
            await this.AssertGetByFrenchId("F2", mapping3);
            await this.AssertGetByFrenchId("F3", mapping4);

            // Delete
            await this.AssertDeleteAll();
        }

        private async Task AssertGet(string id, MappingSimple expected)
        {
            var actual = await this.mappingDataAccess.GetById(id);
            actual.ShouldDeepEqual(expected);
        }

        private async Task AssertGetAll(params MappingSimple[] expected)
        {
            var actual = await this.mappingDataAccess.GetAll();
            actual.ShouldDeepEqual(expected);
        }

        private async Task AssertGetByFrenchId(string frenchId, MappingSimple expected)
        {
            var actual = await this.mappingDataAccess.GetByFrenchId(frenchId);
            actual.ShouldDeepEqual(expected);
        }

        private async Task AssertInsert(MappingSimple mapping)
        {
            this.mappingDataAccess.Insert(mapping);
            await this.AssertGet(mapping.FrenchId, mapping);
        }

        private async Task AssertDeleteAll()
        {
            await this.mappingDataAccess.DeleteAll();
            await this.AssertGetAll();
        }
    }
}