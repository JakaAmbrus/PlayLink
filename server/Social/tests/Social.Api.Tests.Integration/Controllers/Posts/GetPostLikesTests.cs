using Social.Application.Features.Likes.Common;
using Social.Application.Features.Likes.GetPostLikes;

namespace Social.Api.Tests.Integration.Controllers.Posts
{
    [Collection("Sequential")]
    public class GetPostLikesTests : BaseIntegrationTest
    {
        public GetPostLikesTests(IntegrationTestWebAppFactory factory) : base(factory)
        {
        }

        private async Task InitializeTestSeedDataAsync()
        {
            var users = Enumerable.Range(2, 10)
                .Select(i => new AppUser
                {
                    Id = i,
                    UserName = $"{i}",
                    FullName = $"{i} Tester",
                }).ToList();
            Context.Users.AddRange(users);

            Context.Posts.Add(new Post { PostId = 1, AppUserId = 1, LikesCount = 11,});

            Context.Posts.Add(new Post { PostId = 2, AppUserId = 2, LikesCount = 0 });

            var likes = Enumerable.Range(1, 11)
               .Select(i => new Like
               {
                   AppUserId = i,
                   PostId = 1,
               }).ToList();
            Context.Likes.AddRange(likes);

            await Context.SaveChangesAsync();
        }

        private async Task InitializeTestAsync(List<string> roles)
        {
            await ResetDatabaseAsync();
            await InitializeAuthenticatedClient(roles);
            await InitializeTestSeedDataAsync();
        }

        [Fact]
        public async Task GetPostLikes_ShouldReturnCorrectResponseCode_WhenRequestIsValid()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            int postId = 1;
            string url = $"/api/posts/{postId}/likes";

            // Act
            var response = await Client.GetAsync(url);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetPostLikes_ShouldReturnUsersThatLikedThePostWithoutAuthUser_WhenPostExists()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            int postId = 1;
            string url = $"/api/posts/{postId}/likes";

            // Act
            var response = await Client.GetAsync(url);
            var result = await response.Content.ReadFromJsonAsync<GetPostLikesResponse>();

            // Assert
            result.LikedUsers.Count.Should().Be(10);
            result.LikedUsers.Should().AllBeOfType<LikedUserDto>();
            result.LikedUsers.Count.Should().Be(10);
            result.LikedUsers[0].FullName.Should().Be("2 Tester");
            result.LikedUsers[0].Username.Should().Be("2");
        }
    }
}
