namespace DocumentStore.Tests
{
    using System.Linq;
    using System.Threading.Tasks;
    using DeepEqual.Syntax;
    using Model;
    using Model.Tests;
    using Xunit;

    public class MappingComplexDataAccessTests
    {
        private readonly MappingComplexDataAccess mappingDataAccess;

        public MappingComplexDataAccessTests()
        {
            this.mappingDataAccess = new MappingComplexDataAccess(DocumentStoreFactory.CreateDocumentStore(new ConnectionStrings()));
        }

        public static MappingComplex CreateMapping(string worldId, params string[] frenchIds)
        {
            return new MappingComplex
            {
                WorldId   = worldId,
                FrenchIds = frenchIds.ToList()
            };
        }

        [Fact]
        public async Task Should_C_R_U_D()
        {
            var mapping1 = CreateMapping("W1");
            var mapping2 = CreateMapping("W2", "F1");
            var mapping3 = CreateMapping("W3", "F2", "F3");

            // Create
            await this.AssertInsert(mapping1);
            await this.AssertInsert(mapping2);
            await this.AssertInsert(mapping3);

            // Read
            await this.AssertGetAll(mapping1, mapping2, mapping3);
            await this.AssertGetByFrenchId("F1", mapping2);
            await this.AssertGetByFrenchId("F2", mapping3);
            await this.AssertGetByFrenchId("F3", mapping3);
            await this.AssertGetByFrenchIds(new[] {"F1", "F2"}, mapping2, mapping3);

            // Delete
            await this.AssertDeleteAll();
        }

        private async Task AssertGet(string id, MappingComplex expected)
        {
            var actual = await this.mappingDataAccess.GetById(id);
            actual.ShouldDeepEqual(expected);
        }

        private async Task AssertGetAll(params MappingComplex[] expected)
        {
            var actual = await this.mappingDataAccess.GetAll();
            actual.ShouldDeepEqual(expected);
        }

        private async Task AssertGetByFrenchId(string frenchId, MappingComplex expected)
        {
            var actual = await this.mappingDataAccess.GetByFrenchId(frenchId);
            actual.ShouldDeepEqual(expected);
        }

        private async Task AssertGetByFrenchIds(string[] frenchIds, params MappingComplex[] expected)
        {
            var actual = await this.mappingDataAccess.GetByFrenchIds(frenchIds);
            actual.ShouldDeepEqual(expected);
        }

        private async Task AssertInsert(MappingComplex mapping)
        {
            this.mappingDataAccess.Insert(mapping);
            await this.AssertGet(mapping.WorldId, mapping);
        }

        private async Task AssertDeleteAll()
        {
            await this.mappingDataAccess.DeleteAll();
            await this.AssertGetAll();
        }
    }
}