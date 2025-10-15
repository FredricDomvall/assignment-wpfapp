using Infrastructure.Repositories;

namespace Infrastructure.Repository;
public class JsonFileRepository_Tests
{
    private record TestModel(string Name);
    private readonly JsonFileRepository<TestModel> _repository = new();

    [Fact]
    public async Task WriteToJsonFileAsync_Should_Create_File_And_Return_Success()
    {
        // Arrange
        var repositoryTestFile = Path.GetTempFileName();
        var data = new List<TestModel> { new("TestName") };

        // Act
        var result = await _repository.WriteToJsonFileAsync(repositoryTestFile, data);

        // Assert
        Assert.True(result.Statement);
        Assert.True(File.Exists(repositoryTestFile));
        var content = await File.ReadAllTextAsync(repositoryTestFile);
        Assert.Contains("TestName", content);
    }
}
