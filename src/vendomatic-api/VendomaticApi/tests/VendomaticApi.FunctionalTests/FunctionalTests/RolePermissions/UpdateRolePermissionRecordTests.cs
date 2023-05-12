namespace VendomaticApi.FunctionalTests.FunctionalTests.RolePermissions;

using VendomaticApi.SharedTestHelpers.Fakes.RolePermission;
using VendomaticApi.FunctionalTests.TestUtilities;
using VendomaticApi.Domain;
using SharedKernel.Domain;
using FluentAssertions;
using Xunit;
using System.Net;
using System.Threading.Tasks;

public class UpdateRolePermissionRecordTests : TestBase
{
    [Fact]
    public async Task put_rolepermission_returns_nocontent_when_entity_exists_and_auth_credentials_are_valid()
    {
        // Arrange
        var fakeRolePermission = new FakeRolePermissionBuilder().Build();
        var updatedRolePermissionDto = new FakeRolePermissionForUpdateDto().Generate();

        var user = await AddNewSuperAdmin();
        FactoryClient.AddAuth(user.Identifier);
        await InsertAsync(fakeRolePermission);

        // Act
        var route = ApiRoutes.RolePermissions.Put(fakeRolePermission.Id);
        var result = await FactoryClient.PutJsonRequestAsync(route, updatedRolePermissionDto);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
            
    [Fact]
    public async Task put_rolepermission_returns_unauthorized_without_valid_token()
    {
        // Arrange
        var fakeRolePermission = new FakeRolePermissionBuilder().Build();
        var updatedRolePermissionDto = new FakeRolePermissionForUpdateDto { }.Generate();

        // Act
        var route = ApiRoutes.RolePermissions.Put(fakeRolePermission.Id);
        var result = await FactoryClient.PutJsonRequestAsync(route, updatedRolePermissionDto);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
            
    [Fact]
    public async Task put_rolepermission_returns_forbidden_without_proper_scope()
    {
        // Arrange
        var fakeRolePermission = new FakeRolePermissionBuilder().Build();
        var updatedRolePermissionDto = new FakeRolePermissionForUpdateDto { }.Generate();
        FactoryClient.AddAuth();

        // Act
        var route = ApiRoutes.RolePermissions.Put(fakeRolePermission.Id);
        var result = await FactoryClient.PutJsonRequestAsync(route, updatedRolePermissionDto);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}