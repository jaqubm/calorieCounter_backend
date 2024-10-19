CREATE SCHEMA calorieCounter;


CREATE TABLE calorieCounter.[User]
(
    Email NVARCHAR(255) NOT NULL PRIMARY KEY,
    Name NVARCHAR(255) NOT NULL,
);

CREATE TABLE calorieCounter.[Product] (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    ValuesPer FLOAT NOT NULL,
    Energy FLOAT NOT NULL,
    Protein FLOAT NOT NULL,
    Carbohydrates FLOAT NOT NULL,
    Fat FLOAT NOT NULL,
    OwnerEmail NVARCHAR(255),
    FOREIGN KEY (OwnerEmail) REFERENCES calorieCounter.[User](Email) ON DELETE CASCADE
);

CREATE TABLE calorieCounter.[Recipe] (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Instructions NVARCHAR(MAX) NOT NULL,
    OwnerEmail NVARCHAR(255),
    FOREIGN KEY (OwnerEmail) REFERENCES calorieCounter.[User](Email) ON DELETE CASCADE
);

CREATE TABLE calorieCounter.[RecipeProduct] (
    RecipeId INT NOT NULL,
    ProductId INT NOT NULL,
    Weight FLOAT NOT NULL,
    PRIMARY KEY (RecipeId, ProductId),
    FOREIGN KEY (RecipeId) REFERENCES calorieCounter.[Recipe](Id) ON DELETE CASCADE,
    FOREIGN KEY (ProductId) REFERENCES calorieCounter.[Product](Id) ON DELETE NO ACTION 
);

CREATE TABLE calorieCounter.[UserEntry] (
    Id INT PRIMARY KEY IDENTITY(1,1),
    UserEmail NVARCHAR(255) NOT NULL,
    EntryType NVARCHAR(10) CHECK (EntryType IN ('product', 'recipe')),
    ProductId INT NULL,
    RecipeId INT NULL,
    Date DATE NOT NULL,
    MealType NVARCHAR(20) CHECK (MealType IN ('Breakfast', 'Lunch', 'Dessert', 'Dinner')),
    weight FLOAT NULL,
    FOREIGN KEY (UserEmail) REFERENCES calorieCounter.[User](Email) ON DELETE CASCADE,
    FOREIGN KEY (ProductId) REFERENCES calorieCounter.[Product](Id) ON DELETE NO ACTION,
    FOREIGN KEY (RecipeId) REFERENCES calorieCounter.[Recipe](Id) ON DELETE NO ACTION
);


DROP TABLE calorieCounter.[User];
DROP TABLE calorieCounter.[Product];
DROP TABLE calorieCounter.[Recipe];
DROP TABLE calorieCounter.[RecipeProduct];
DROP TABLE calorieCounter.[UserEntry];