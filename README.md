# ExchangeRateAPI

1)	The program is written by using Visual Studio 2019.
2)	Open Visual Studio, and import project solution file cloned from link repository.
3)	You need to have .NET 6.0 installed on the machine you are running Visual studio.
4)	After importing project you should be able to see on the right hand side project structure.
5)	Run the project using green arrow.
6)	After executing the project, you will need some tool for testing WEB API (for example Postman).
8)	Select POST as Action method in Postman.
9)  URL endpoint for accessing WEB API is: https://localhost:44301/api/historicalexchangerate/rates?baseCurrency=SEK&targetCurrency=NOK
    **Note**: The port can be changed according to your preferences.
    
   - You should provide date array as JSON object in the format as shown below:

```json
{
    "date": {
        "data": [
            "2018-02-01",
            "2018-02-15",
            "2018-03-01"
        ]
    }
}
```    
   
   - After calling the API, you will get result in JSON object as shown below:
   
```json
 {
    "data": {
        "avg": "An average rate of 0.970839476467",
        "max": "A max rate of 0.9815486993 on 2018-02-15",
        "min": "A min rate of 0.9546869595 on 2018-03-01"
    },
    "result": "Success"
}
```
<details><summary>Test case 1 - Empty list of dates</summary>
<p>

Providing list of dates is mandatory, in order to get valid result.  Otherwise you will get an error of bad request. 
In the case you provide empty array, for an example:
   
  ```json
{
    "date": {
        "data": []
    }
}
  ```
  You will get custom error message JSON object as result, as shown below:
  ```json
  {
    		"Date issue": [
        		"The provided date set should consist of at least one date value."
    		]
  }
  ```

</p>
</details>

  
<details><summary>Test case 2 - Incorrect date format</summary>
<p>
   If you provide incorrect date item, as shown below:
  
```json
{
    "date": {
        "data": [
            "2022-CRAYON"
        ]
    }
}
```
  You will get custom error message JSON object as result, as shown below:
  ```json
  {
    "errors": {
        "listDate": [
            "The listDate field is required."
        ],
        "date.data[0]": [
            "Could not convert string to DateTime: 2022-CRAYON. Path 'date.data[0]', line 4, position 25."
        ]
    },
    "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
    "title": "One or more validation errors occurred.",
    "status": 400,
    "traceId": "00-d9cc3f0be8566032d0852aba55244abb-b39f48401d170b88-00"
  }
  ```
</p>
</details>

<details><summary>Test case 3 - Remote service does not have data for provided date</summary>
<p>
If the remote service does not have data for any of the dates in the list of dates:
   
```json
{
    "date": {
        "data": [
            "1990-04-01",
            "1990-04-15"
        ]
    }
```   
   
You will receive the following error message:


   
```json
  {
    "Remote service issue": [
        "Corrupted data from remote server."
    ]
  }
```
</p>
</details>  

<details><summary>Test case 4 - Sending date ahead</summary>
<p>
 If you are sending date after the current date:
 
```json
  {
    "date": {
        "data": 
        [
            "2023-01-01"
        ]
    }
  }
```
  You will receive the following error message:
 
```json
  {
    "Date issue": [
        "Cannot fetch data for dates ahead. Date ahead: 2023-01-01"
    ]
 }
```
</p>
</details>  
   
 
