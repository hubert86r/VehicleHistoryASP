# VehicleHistoryASP
Opis wytworzenia systemu informacyjnego
1.	Założenia ( cel ,kontekst, zakres tworzonego systemu)
W ramach projektu wykonana zostanie aplikacja pomagająca w obsłudze technicznej samochodu. Będzie w niej można sprawdzić aktualny przebieg, kiedy trzeba będzie wymienić olej, datę przeglądu technicznego, spalanie i wiele innych parametrów naszego samochodu.
Aplikacja będzie posiadała interfejs za pomocą, którego będzie można wprowadzać informację do bazy danych, edytować je oraz wyświetlać raporty. 
2.	Aktorzy systemu – klasy użytkowników
Użytkownik posiadający jeden samochód lub więcej.
3.	Wymagania funkcjonalne – w rozbiciu na klasy użytkowników
Użytkownik:
- dodanie samochody
- rejestracja tankowania samochodu
- rejestracja czy serwisowej + dodanie punktu serwisowego jeśli go nie było 


4. Baza danych triggery:

CREATE TRIGGER [dbo].[fuel_usage_calculation]
ON [dbo].[Refueling_History]
FOR INSERT
AS BEGIN

DECLARE @fuel_quantity decimal(5,2)
		,@milage_new INT
		,@milage_old INT
		,@update_date datetime
		,@id_car INT
		,@id_refuling INT
		,@fuel_usage MONEY

		SELECT @fuel_quantity = fuel_quantity
		,@milage_new = milage
		,@id_car = id_car
		,@id_refuling = id_refuling 
		,@update_date = date
		FROM inserted

		--SELECT top 1 @milage_old = ISNULL(a.milage,1) from (select top 2 date,  milage from Refueling_History where id_car= @id_car order by date desc) a order by date
		
		SET @milage_old = (SELECT top 1 a.milage from (select top 2 date,  milage from Refueling_History where id_car= @id_car order by date desc) a order by date)
		
		SET @fuel_usage = (@fuel_quantity * 100)/NULLIF((@milage_new - @milage_old),0)

		UPDATE Refueling_History SET fuel_usage = @fuel_usage  
		WHERE id_refuling = @id_refuling;

		UPDATE Cars SET actual_mileage = @milage_new, update_date = @update_date, fuel_usage = @fuel_usage
		WHERE id_car = @id_car;
END
GO


CREATE TRIGGER [dbo].[car_service_update]
ON [dbo].[Service_History]
FOR INSERT
AS BEGIN

DECLARE 
		 @service_type INT
		,@milage INT
		,@update_date datetime
		,@id_car INT

		SELECT @service_type = id_type
		,@milage = milage
		,@id_car = id_car
		,@update_date = date 
		FROM inserted

		IF @service_type = 4 --wymiana oleju
		BEGIN
		UPDATE Cars SET actual_mileage = @milage, update_date = @update_date, oil_date = DATEADD(YEAR, 1, @update_date), oil_milage = @milage +15000 WHERE id_car = @id_car
		END
		ELSE IF @service_type = 5 --wymiana rozrządu
		BEGIN
		UPDATE Cars SET actual_mileage = @milage, update_date = @update_date, timing_date = DATEADD(YEAR, 5, @update_date), timing_milage = @milage +80000 WHERE id_car = @id_car
		END
END
GO
