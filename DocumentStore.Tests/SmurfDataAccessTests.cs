namespace DocumentStore.Tests
{
    using System;
    using System.Threading.Tasks;
    using DeepEqual.Syntax;
    using Model;
    using Model.Tests;
    using Xunit;

    public class SmurfDataAccessTests
    {
        private readonly SmurfDataAccess smurfDataAccess;

        public SmurfDataAccessTests()
        {
            this.smurfDataAccess = new SmurfDataAccess(DocumentStoreFactory.CreateDocumentStore(new ConnectionStrings()));
        }

        [Fact]
        public async Task Should_C_R_U_D()
        {
            var smurf1 = new Smurf       {Ability = "smurf"};
            var smurf2 = new SmurfLeader {Ability = "Lead"};
            var smurf3 = new PapaSmurf   {Ability = "Papa",    Rank = 1, Age = 100};
            var smurf4 = new Smurfette   {Ability = "Amuse",   Suitors = 50};
            var smurf5 = new HeftySmurf  {Ability = "Protect", Rank = 2, Weight = 300};
            var smurf6 = new BrainySmurf {Ability = "Invent",  QI = 125};

            // Create
            await this.AssertSave(smurf1);
            await this.AssertSave(smurf2);
            await this.AssertSave(smurf3);
            await this.AssertSave(smurf4);
            await this.AssertSave(smurf5);
            await this.AssertSave(smurf6);

            // Read
            await this.AssertGetAll(smurf1, smurf2, smurf3, smurf4, smurf5, smurf6);
            await this.AssertGetLeaders(null, smurf2, smurf3, smurf5);
            await this.AssertGetLeaders(1, smurf2, smurf3);

            // Update
            smurf1.Ability = "none";
            await this.AssertSave(smurf1);

            // Delete
            await this.AssertDelete(smurf1);
            await this.AssertDelete(smurf2);
            await this.AssertDelete(smurf3);
            await this.AssertDelete(smurf4);
            await this.AssertDelete(smurf5);
            await this.AssertDelete(smurf6);
        }

        private async Task AssertGet(Guid id, Smurf expected)
        {
            var actual = await this.smurfDataAccess.GetById(id);
            actual.ShouldDeepEqual(expected);
        }

        private async Task AssertGetAll(params Smurf[] expected)
        {
            var actual = await this.smurfDataAccess.GetAll();
            actual.ShouldDeepEqual(expected);
        }

        private async Task AssertGetLeaders(int? maxRank, params Smurf[] expected)
        {
            var actual = await this.smurfDataAccess.GetLeaders(maxRank);
            actual.ShouldDeepEqual(expected);
        }

        private async Task AssertSave(Smurf smurf)
        {
            await this.smurfDataAccess.Save(smurf);
            await this.AssertGet(smurf.Id, smurf);
        }

        private async Task AssertDelete(Smurf smurf)
        {
            this.smurfDataAccess.Delete(smurf);
            await this.AssertGet(smurf.Id, null);
        }
    }
}