# PdfApi
- Starta upp lokalt i VS/vscode
- Importera och kör nedan till Postman
```json
{
	"info": {
		"_postman_id": "4321acd7-0fc9-4681-8420-2aa65ea98c55",
		"name": "LOCAL TMP",
		"schema": "https://schema.getpostman.com/json/collection/v2.1.0/collection.json",
		"_exporter_id": "44216214",
		"_collection_link": "https://uffe-517000.postman.co/workspace/Uffe's-Workspace~4fb46885-2d5e-4f34-8d64-04977d5f37db/collection/44216214-4321acd7-0fc9-4681-8420-2aa65ea98c55?action=share&source=collection_link&creator=44216214"
	},
	"item": [
		{
			"name": "CreatePDF",
			"request": {
				"method": "POST",
				"header": [],
				"body": {
					"mode": "raw",
					"raw": "{\r\n  \"content\": \"<!DOCTYPE html>\\n<html lang=\\\"sv\\\">\\n<head>\\n  <meta charset=\\\"UTF-8\\\">\\n  <title>Exempelsida</title>\\n  <style>\\n    body { font-family: Arial, sans-serif; background-color: #f8f8f8; }\\n    h1 { color: #2a6; }\\n  </style>\\n</head>\\n<body>\\n  <h1>Hej världen!</h1>\\n  <p>Detta är ett <strong>exempel</strong> på en HTML-sida som skickas i JSON-body.</p>\\n  <p>Innehåller svenska tecken: å, ä, ö — samt specialtecken: &lt;, &gt;, &amp;.</p>\\n</body>\\n</html>\"\r\n}",
					"options": {
						"raw": {
							"language": "json"
						}
					}
				},
				"url": {
					"raw": "https://localhost:7205/api/createpdf?title=test",
					"protocol": "https",
					"host": [
						"localhost"
					],
					"port": "7205",
					"path": [
						"api",
						"createpdf"
					],
					"query": [
						{
							"key": "title",
							"value": "test"
						}
					]
				}
			},
			"response": []
		}
	]
}
```