CREATE TABLE Train (
    Train_ID TINYINT PRIMARY KEY AUTO_INCREMENT,
    Train_Number VARCHAR(10) NOT NULL UNIQUE,
    Departure_Station VARCHAR(255) NOT NULL,
    Destination_Station VARCHAR(255) NOT NULL,
    Number_of_Cars INT NOT NULL,
    Car_Types VARCHAR(255) NOT NULL,
    Number_of_Coupe INT NOT NULL,
    Number_of_Plac INT NOT NULL
);

CREATE TABLE Seats (
    Seats_ID TINYINT PRIMARY KEY AUTO_INCREMENT,
    Train_Number VARCHAR(10) NOT NULL,
    Car_Number INT NOT NULL,
    Car_Type VARCHAR(20) NOT NULL,
    Seat_Code VARCHAR(10) NOT NULL,
    CONSTRAINT fk_train_number_schedule FOREIGN KEY (Train_Number) REFERENCES Train(Train_Number)
);

CREATE TABLE Route (
    Route_ID SMALLINT PRIMARY KEY AUTO_INCREMENT,
    Train_ID TINYINT,
    Station_ID TINYINT,
    Route_Length DECIMAL(10, 2),
    Price_coupe DECIMAL(10, 2) NOT NULL,
    Price_plac DECIMAL(10, 2) NOT NULL,
    CONSTRAINT fk_train FOREIGN KEY (Train_ID) REFERENCES Train(Train_ID),
    CONSTRAINT fk_station FOREIGN KEY (Station_ID) REFERENCES Station(Station_ID)
);

CREATE TABLE Schedule (
    Schedule_ID TINYINT PRIMARY KEY AUTO_INCREMENT,
    Train_Number VARCHAR(10),
    Platform VARCHAR(50),
    Departure_Date DATE NOT NULL,
    Departure_Time TIME NOT NULL,
    CONSTRAINT fk_train_number FOREIGN KEY (Train_Number) REFERENCES Train(Train_Number),
    UNIQUE (Train_Number, Departure_Date, Departure_Time) -- Add this line to create a composite unique index
);

CREATE TABLE Station (
    Station_ID TINYINT PRIMARY KEY AUTO_INCREMENT,
    Station_Name VARCHAR(255) NOT NULL,
    City VARCHAR(255) NOT NULL
);

CREATE TABLE Ticket_Sales (
    Sale_ID TINYINT PRIMARY KEY AUTO_INCREMENT,
    Ticket_Code VARCHAR(10) NOT NULL UNIQUE,
    Route_ID SMALLINT,
    Platform VARCHAR(50) NOT NULL,
    Train_Number VARCHAR(10) NOT NULL,
    Сar_Number tinyint not null,
    Seat_Type VARCHAR(20) NOT NULL,
    Seat_Number INT NOT NULL,
    Departure_Date DATE NOT NULL,
    Departure_Time TIME NOT NULL,
    Price DECIMAL(10, 2) NOT NULL,
    Customer_Name VARCHAR(100) NOT NULL,
    CONSTRAINT fk_route_id FOREIGN KEY (Route_ID) REFERENCES Route(Route_ID),
    CONSTRAINT fk_train_number_sales FOREIGN KEY (Train_Number, Departure_Date, Departure_Time) REFERENCES Schedule(Train_Number, Departure_Date, Departure_Time)
);

CREATE TABLE Users_log (
    User_ID INT PRIMARY KEY AUTO_INCREMENT,
    Username VARCHAR(255) NOT NULL UNIQUE,
    Password VARCHAR(255) NOT NULL,
    Role ENUM('Admin', 'Cashier') NOT NULL
);




-- для реалізації щоб не було повторного квитка
ALTER TABLE Ticket_Sales
ADD CONSTRAINT uq_ticket UNIQUE (Train_Number, Seat_Type, Seat_Number, Departure_Date, Departure_Time);

ALTER TABLE Ticket_Sales
ADD column Car_Number tinyint not null;

CREATE INDEX idx_train_date_time ON Schedule (Train_Number, Departure_Date, Departure_Time);

DROP TABLE Train;
DROP TABLE Route;
DROP TABLE Schedule;
DROP TABLE Station;
DROP TABLE Ticket_Sales;
DROP TABLE Monthly_Schedule;

ALTER TABLE Schedule
ADD COLUMN Busy_Coupe INT NOT NULL,
ADD COLUMN Busy_Plac INT NOT NULL;

ALTER TABLE Train
DROP COLUMN Number_of_Seats;

ALTER TABLE Train
ADD COLUMN Number_of_Coupe INT NOT NULL,
ADD COLUMN Number_of_Plac INT NOT NULL;

ALTER TABLE Monthly_Schedule
DROP COLUMN Departure_Date,
DROP COLUMN Departure_Time;