CREATE SCHEMA calorieCounter;


CREATE TABLE calorieCounter.[User]
(
    Id NVARCHAR(21) NOT NULL PRIMARY KEY,
    Email NVARCHAR(255) NOT NULL UNIQUE,
    Name NVARCHAR(255) NOT NULL,
    Energy REAL NOT NULL,
    Protein REAL NOT NULL,
    Carbohydrates REAL NOT NULL,
    Fat REAL NOT NULL
);

CREATE TABLE calorieCounter.[Product] (
    Id NVARCHAR(50) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    ValuesPer REAL NOT NULL,
    Energy REAL NOT NULL,
    Protein REAL NOT NULL,
    Carbohydrates REAL NOT NULL,
    Fat REAL NOT NULL,
    OwnerId NVARCHAR(21),
    FOREIGN KEY (OwnerId) REFERENCES calorieCounter.[User](Id) ON DELETE CASCADE
);

CREATE TABLE calorieCounter.[Recipe] (
    Id NVARCHAR(50) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Instructions NVARCHAR(MAX) NOT NULL,
    OwnerId NVARCHAR(21),
    FOREIGN KEY (OwnerId) REFERENCES calorieCounter.[User](Id) ON DELETE CASCADE
);

CREATE TABLE calorieCounter.[RecipeProduct] (
    RecipeId NVARCHAR(50) NOT NULL,
    ProductId NVARCHAR(50) NOT NULL,
    Weight REAL NOT NULL,
    PRIMARY KEY (RecipeId, ProductId),
    FOREIGN KEY (RecipeId) REFERENCES calorieCounter.[Recipe](Id) ON DELETE CASCADE,
    FOREIGN KEY (ProductId) REFERENCES calorieCounter.[Product](Id) ON DELETE NO ACTION
);

CREATE TABLE calorieCounter.[UserEntry] (
    Id NVARCHAR(50) PRIMARY KEY,
    UserId NVARCHAR(21) NOT NULL,
    EntryType NVARCHAR(10) CHECK (EntryType IN ('product', 'recipe')),
    ProductId NVARCHAR(50) NULL,
    RecipeId NVARCHAR(50) NULL,
    Date DATE NOT NULL,
    MealType NVARCHAR(20) CHECK (MealType IN ('Breakfast', 'Lunch', 'Dessert', 'Dinner')),
    Weight REAL NULL,
    FOREIGN KEY (UserId) REFERENCES calorieCounter.[User](Id) ON DELETE CASCADE,
    FOREIGN KEY (ProductId) REFERENCES calorieCounter.[Product](Id) ON DELETE NO ACTION,
    FOREIGN KEY (RecipeId) REFERENCES calorieCounter.[Recipe](Id) ON DELETE NO ACTION
);


DROP TABLE calorieCounter.[RecipeProduct];
DROP TABLE calorieCounter.[UserEntry];
DROP TABLE calorieCounter.[Recipe];
DROP TABLE calorieCounter.[Product];
DROP TABLE calorieCounter.[User];