using Infrastructure.Repositories;

namespace Infrastructure.Tests.Repository;
public class JsonFileRepository_Tests
{
    private record TestModel(string Name);
    private readonly JsonFileRepository<TestModel> _repository = new();

    [Fact]
    public async Task WriteToJsonFileAsync_ShouldCreateFileWhenSuccessful_AndReturnTrue()
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
    [Fact]
    public async Task ReadFromJsonFileAsync_FileExists_ShouldReturnDeserializedData()
    {
        // Arrange
        var repositoryTestFile = Path.GetTempFileName();
        var data = new List<TestModel> { new("Example") };
        await _repository.WriteToJsonFileAsync(repositoryTestFile, data);

        // Act
        var result = await _repository.ReadFromJsonFileAsync(repositoryTestFile);

        // Assert
        Assert.True(result.Statement);
        Assert.Single(result.Outcome!);
        Assert.Equal("Example", result.Outcome![0].Name);
    }

    [Fact]
    public async Task ReadFromJsonFileAsync_FileMissing_ShouldReturnFalseAndEmptyList()
    {
        // Arrange
        var repositoryTestFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".json");

        // Act
        var result = await _repository.ReadFromJsonFileAsync(repositoryTestFile);

        // Assert
        Assert.False(result.Statement);
        Assert.Empty(result.Outcome!);
        Assert.Equal("File doesnt exist", result.Answer);
    }

    [Fact]
    public async Task ReadFromJsonFileAsync_FileIsEmpty_ShouldReturnFalseAndFileIsEmptyMessage()
    {
        // Arrange
        var repositoryTestFile = Path.GetTempFileName();
        await File.WriteAllTextAsync(repositoryTestFile, string.Empty);

        // Act
        var result = await _repository.ReadFromJsonFileAsync(repositoryTestFile);

        // Assert
        Assert.False(result.Statement);
        Assert.Equal("File is empty", result.Answer);
    }
}
