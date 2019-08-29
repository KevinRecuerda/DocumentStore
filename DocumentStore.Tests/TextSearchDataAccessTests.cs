namespace DocumentStore.Tests
{
    using System.Threading.Tasks;
    using DeepEqual.Syntax;
    using Model;
    using Model.Tests;
    using Xunit;

    public class TextSearchDataAccessTests
    {
        private readonly TextSearchDataAccess dataAccess;

        public TextSearchDataAccessTests()
        {
            this.dataAccess = new TextSearchDataAccess(DocumentStoreFactory.CreateDocumentStore(new ConnectionStrings()));
        }

        public static TextSearch CreateTextSearch(string text)
        {
            return new TextSearch
            {
                Text = text
            };
        }

        [Fact]
        public async Task Should_C_R_U_D()
        {
            var text1 = CreateTextSearch("something occured");
            var text2 = CreateTextSearch("something else occured");
            var text3 = CreateTextSearch("something is wrong");
            var text4 = CreateTextSearch("this is wrong");

            // Create
            await this.AssertSave(text1);
            await this.AssertSave(text2);
            await this.AssertSave(text3);
            await this.AssertSave(text4);

            // Read
            await this.AssertGetAll(text1, text2, text3, text4);
            await this.AssertSearchByText("som", text1, text2, text3);
            await this.AssertSearchByText("occ", text1, text2);
            await this.AssertSearchByText("rong", text3, text4);
            await this.AssertSearchByText("WrONg", text3, text4);
            await this.AssertSearchByText("thi o", text1, text2, text3, text4);
            await this.AssertSearchByText("is", text3, text4, text2);

            // Delete
            await this.AssertDelete(text1);
            await this.AssertDelete(text2);
            await this.AssertDelete(text3);
            await this.AssertDelete(text4);
        }

        private async Task AssertGet(long id, TextSearch expected)
        {
            var actual = await this.dataAccess.GetById(id);
            actual.ShouldDeepEqual(expected);
        }

        private async Task AssertGetAll(params TextSearch[] expected)
        {
            var actual = await this.dataAccess.GetAll();
            actual.ShouldDeepEqual(expected);
        }

        private async Task AssertSearchByText(string search, params TextSearch[] expected)
        {
            var actual = await this.dataAccess.SearchByText(search);
            actual.ShouldDeepEqual(expected);
        }

        private async Task AssertSave(TextSearch text)
        {
            await this.dataAccess.Save(text);
            await this.AssertGet(text.Id, text);
        }

        public async Task AssertDelete(TextSearch text)
        {
            await this.dataAccess.Delete(text);
            await this.AssertGet(text.Id, null);
        }
    }
}