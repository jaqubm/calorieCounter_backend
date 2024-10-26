CREATE SCHEMA calorieCounter;


CREATE TABLE calorieCounter.[User]
(
    Email NVARCHAR(255) NOT NULL PRIMARY KEY,
    Name NVARCHAR(255) NOT NULL,
);

CREATE TABLE calorieCounter.[Product] (
    Id NVARCHAR(50) PRIMARY KEY,
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
    Id NVARCHAR(50) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Instructions NVARCHAR(MAX) NOT NULL,
    OwnerEmail NVARCHAR(255),
    FOREIGN KEY (OwnerEmail) REFERENCES calorieCounter.[User](Email) ON DELETE CASCADE
);

CREATE TABLE calorieCounter.[RecipeProduct] (
    RecipeId NVARCHAR(50) NOT NULL,
    ProductId NVARCHAR(50) NOT NULL,
    Weight FLOAT NOT NULL,
    PRIMARY KEY (RecipeId, ProductId),
    FOREIGN KEY (RecipeId) REFERENCES calorieCounter.[Recipe](Id) ON DELETE CASCADE,
    FOREIGN KEY (ProductId) REFERENCES calorieCounter.[Product](Id) ON DELETE NO ACTION 
);

CREATE TABLE calorieCounter.[UserEntry] (
    Id NVARCHAR(50) PRIMARY KEY,
    UserEmail NVARCHAR(255) NOT NULL,
    EntryType NVARCHAR(10) CHECK (EntryType IN ('product', 'recipe')),
    ProductId NVARCHAR(50) NULL,
    RecipeId NVARCHAR(50) NULL,
    Date DATE NOT NULL,
    MealType NVARCHAR(20) CHECK (MealType IN ('Breakfast', 'Lunch', 'Dessert', 'Dinner')),
    Weight FLOAT NULL,
    FOREIGN KEY (UserEmail) REFERENCES calorieCounter.[User](Email) ON DELETE CASCADE,
    FOREIGN KEY (ProductId) REFERENCES calorieCounter.[Product](Id) ON DELETE NO ACTION,
    FOREIGN KEY (RecipeId) REFERENCES calorieCounter.[Recipe](Id) ON DELETE NO ACTION
);


DROP TABLE calorieCounter.[Recipe];
DROP TABLE calorieCounter.[RecipeProduct];
DROP TABLE calorieCounter.[UserEntry];
DROP TABLE calorieCounter.[Product];
DROP TABLE calorieCounter.[User];