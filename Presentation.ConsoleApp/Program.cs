using Infrastructure.Models;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Presentation.ConsoleApp.Menus;

string jsonFileSource = "products.json";

JsonFileRepository<Product> jsonFileRepository = new JsonFileRepository<Product>(jsonFileSource);
ProductService productService = new ProductService(jsonFileRepository);
ProductMenu productMenu = new ProductMenu(productService);
MainMenu mainMenu = new MainMenu(productMenu);

await mainMenu.Run();
