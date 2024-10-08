-- DB HERE

CREATE SCHEMA calorieCounter;


CREATE TABLE calorieCounter.[User]
(
    Email NVARCHAR(255) NOT NULL PRIMARY KEY,   -- Corresponds to 'Email' property
    Name NVARCHAR(255) NOT NULL,                -- Corresponds to 'Name' property
);