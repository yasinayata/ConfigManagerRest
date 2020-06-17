1-) Install Swashbuckle.aspnetcore (NuGet Packages)
	http://localhost:20281/swagger/index.html

2-) Use Postman (or any client application)

	# Address : http://localhost:20281/api/config/ping
	# Request : GET	
	
	Response :
	{
		"result": true,
		"message": "Successful"
	}

	# =========================================================================

	# Address : http://localhost:20281/api/config/login
	# Request : POST
	{
		"Username" : "user@gmail.com",
		"Password" : "12345"
	}

	Response :
	{
		"data": {
			"sessionId": "0c6f6096-86af-4cd7-9321-220f70f7ac5a",
			"token": "QR7lyJIS2Eg2OE5qa093bHdxUmMwQS9zZ3hzRGVVS3U0SGVreXhML09BT1owUUJzZVcxNmx4bi9OL00yUFNyZkhGRlBiYUJzYXNJUGNqZ1VNZmhmbnhvWUJPeDdpV0hSVWUvblNHa0FuVGJQVnRGN1g2clNZSXlhQm5xSjZrVnFEekJpWHAxa2g2dHZiZmFTUnduMDRtMUxza1ZPNWM5NnNYaktSNUdGakNwMW9Yd3NNN2E2YmsxbFQvTVppeW9hOHdRQnN4c3JMZG5RSXhFYWpjSkNlZnFic3VjOVRTb0gvY3I4V1Qvd3NEL1pMckN3cE9xTHBKaFpDYk9ORlFWVnNtU2VoNVljL1YzdGNrUS91WUMreXR0empkdis4QT09"
		},
		"result": true,
		"message": "Successful"
	}

	# =========================================================================

	# Address : http://localhost:20281/api/config/get
	# Request : POST
	{
		SessionUser : 
			{
				"sessionId": "0c6f6096-86af-4cd7-9321-220f70f7ac5a",
				"token": "QR7lyJIS2Eg2OE5qa093bHdxUmMwQS9zZ3hzRGVVS3U0SGVreXhML09BT1owUUJzZVcxNmx4bi9OL00yUFNyZkhGRlBiYUJzYXNJUGNqZ1VNZmhmbnhvWUJPeDdpV0hSVWUvblNHa0FuVGJQVnRGN1g2clNZSXlhQm5xSjZrVnFEekJpWHAxa2g2dHZiZmFTUnduMDRtMUxza1ZPNWM5NnNYaktSNUdGakNwMW9Yd3NNN2E2YmsxbFQvTVppeW9hOHdRQnN4c3JMZG5RSXhFYWpjSkNlZnFic3VjOVRTb0gvY3I4V1Qvd3NEL1pMckN3cE9xTHBKaFpDYk9ORlFWVnNtU2VoNVljL1YzdGNrUS91WUMreXR0empkdis4QT09"
			},
		Parameter :
			{
				"Guid" : "e71841eb-fd19-4e78-b79a-8b293f27ade3",
				"Username" : "ConfigManagerService@gmail.com",
				"Application" : "ConfigManagerService",
				"KeyName" : "Debug"
			}
	}

	# Response :
	{
		"data": {
			"guid": "e71841eb-fd19-4e78-b79a-8b293f27ade3",
			"username": "ConfigManagerService@gmail.com",
			"application": "ConfigManagerService",
			"keyName": "Debug",
			"keyValue": "1",
			"notes": ""
		},
		"result": true,
		"message": "Successful"
	}

	# =========================================================================

	# Address : http://localhost:20281/api/config/put
	# Request : POST
	{
		SessionUser : 
			{
				"sessionId": "0c6f6096-86af-4cd7-9321-220f70f7ac5a",
				"token": "QR7lyJIS2Eg2OE5qa093bHdxUmMwQS9zZ3hzRGVVS3U0SGVreXhML09BT1owUUJzZVcxNmx4bi9OL00yUFNyZkhGRlBiYUJzYXNJUGNqZ1VNZmhmbnhvWUJPeDdpV0hSVWUvblNHa0FuVGJQVnRGN1g2clNZSXlhQm5xSjZrVnFEekJpWHAxa2g2dHZiZmFTUnduMDRtMUxza1ZPNWM5NnNYaktSNUdGakNwMW9Yd3NNN2E2YmsxbFQvTVppeW9hOHdRQnN4c3JMZG5RSXhFYWpjSkNlZnFic3VjOVRTb0gvY3I4V1Qvd3NEL1pMckN3cE9xTHBKaFpDYk9ORlFWVnNtU2VoNVljL1YzdGNrUS91WUMreXR0empkdis4QT09"
			},
		Parameter :
			{
				"Guid" : "e71841eb-fd19-4e78-b79a-8b293f27ade3",
				"Username" : "ConfigManagerService@gmail.com",
				"Application" : "ConfigManagerService",
				"KeyName" : "Debug",
				"keyValue": "1 2 3 4 5"
			}
	}

	Response :
	{
		"data": {
			"guid": "e71841eb-fd19-4e78-b79a-8b293f27ade3",
			"username": "ConfigManagerService@gmail.com",
			"application": "ConfigManagerService",
			"keyName": "Debug",
			"keyValue": "1 2 3 4 5",
			"notes": ""
		},
		"result": true,
		"message": "Successful"
	}

	# =========================================================================

	# Address : http://localhost:20281/api/config/list
	# Request : POST
	{
	SessionUser :
		{
			"sessionId": "0c6f6096-86af-4cd7-9321-220f70f7ac5a",
			"token": "QR7lyJIS2Eg2OE5qa093bHdxUmMwQS9zZ3hzRGVVS3U0SGVreXhML09BT1owUUJzZVcxNmx4bi9OL00yUFNyZkhGRlBiYUJzYXNJUGNqZ1VNZmhmbnhvWUJPeDdpV0hSVWUvblNHa0FuVGJQVnRGN1g2clNZSXlhQm5xSjZrVnFEekJpWHAxa2g2dHZiZmFTUnduMDRtMUxza1ZPNWM5NnNYaktSNUdGakNwMW9Yd3NNN2E2YmsxbFQvTVppeW9hOHdRQnN4c3JMZG5RSXhFYWpjSkNlZnFic3VjOVRTb0gvY3I4V1Qvd3NEL1pMckN3cE9xTHBKaFpDYk9ORlFWVnNtU2VoNVljL1YzdGNrUS91WUMreXR0empkdis4QT09"
		},
	Parameter :
		{
			"Guid" : "e71841eb-fd19-4e78-b79a-8b293f27ade3",
			"Username" : "ConfigManagerService@gmail.com",
			"Application" : "ConfigManagerService",
			"KeyName" : "Prm2"
		}
	}

	Response :
	{
		"data": [
			{
				"guid": "e71841eb-fd19-4e78-b79a-8b293f27ade3",
				"username": "ConfigManagerService@gmail.com",
				"application": "ConfigManagerService",
				"keyName": "Debug",
				"keyValue": "eWBb2p1Zi4x1G8dbmb5vnA==",
				"insertDateTime": 1580382002,
				"notes": "",
				"lastProcessDateTime": 1580382019,
				"isDeleted": 0
			},
			{
				"guid": "e71841eb-fd19-4e78-b79a-8b293f27ade3",
				"username": "ConfigManagerService@gmail.com",
				"application": "ConfigManagerService",
				"keyName": "MultiThreadOperation",
				"keyValue": "TW1mZ/5HG88=",
				"insertDateTime": 1591022315,
				"notes": "",
				"lastProcessDateTime": 1591022315,
				"isDeleted": 0
			},
		   ...
		],
		"result": true,
		"message": "Successful"
	}

	# =========================================================================

	# Address : http://localhost:20281/api/config/delete
	# Request : POST
	{
	SessionUser :
		{
			"sessionId": "0c6f6096-86af-4cd7-9321-220f70f7ac5a",
			"token": "QR7lyJIS2Eg2OE5qa093bHdxUmMwQS9zZ3hzRGVVS3U0SGVreXhML09BT1owUUJzZVcxNmx4bi9OL00yUFNyZkhGRlBiYUJzYXNJUGNqZ1VNZmhmbnhvWUJPeDdpV0hSVWUvblNHa0FuVGJQVnRGN1g2clNZSXlhQm5xSjZrVnFEekJpWHAxa2g2dHZiZmFTUnduMDRtMUxza1ZPNWM5NnNYaktSNUdGakNwMW9Yd3NNN2E2YmsxbFQvTVppeW9hOHdRQnN4c3JMZG5RSXhFYWpjSkNlZnFic3VjOVRTb0gvY3I4V1Qvd3NEL1pMckN3cE9xTHBKaFpDYk9ORlFWVnNtU2VoNVljL1YzdGNrUS91WUMreXR0empkdis4QT09"
		},
	Parameter :
		{
			"Guid" : "e71841eb-fd19-4e78-b79a-8b293f27ade3",
			"Username" : "ConfigManagerService@gmail.com",
			"Application" : "ConfigManagerService",
			"KeyName" : "Prm2"
		}
	}

	Response :
	{
		"data": null,
		"result": true,
		"message": "Successful"
	}
	or
	{
		"data": null,
		"result": false,
		"message": "No record"
	}

	# =========================================================================

	# Address : http://localhost:20281/api/config/usercheck
	# Request : POST
	{
			"sessionId": "0c6f6096-86af-4cd7-9321-220f70f7ac5a",
			"token": "QR7lyJIS2Eg2OE5qa093bHdxUmMwQS9zZ3hzRGVVS3U0SGVreXhML09BT1owUUJzZVcxNmx4bi9OL00yUFNyZkhGRlBiYUJzYXNJUGNqZ1VNZmhmbnhvWUJPeDdpV0hSVWUvblNHa0FuVGJQVnRGN1g2clNZSXlhQm5xSjZrVnFEekJpWHAxa2g2dHZiZmFTUnduMDRtMUxza1ZPNWM5NnNYaktSNUdGakNwMW9Yd3NNN2E2YmsxbFQvTVppeW9hOHdRQnN4c3JMZG5RSXhFYWpjSkNlZnFic3VjOVRTb0gvY3I4V1Qvd3NEL1pMckN3cE9xTHBKaFpDYk9ORlFWVnNtU2VoNVljL1YzdGNrUS91WUMreXR0empkdis4QT09"
	}

	Response-1 :
	{
		"result": true,
		"message": "Successful"
	}

	Response-2 :
	{
		"result": false,
		"message": "Unknown User - SessionId : e71841eb-fd19-4e78-b79a-8b293f27ade3"
	}

	Response-3 :
	{
		"result": false,
		"message": "Invalid Token : IkfGKo0I2Eg2OE5qa093bHdxUmMwQS9zZ3hzRGVV3U0SGVreXhML3JCdDNaTFI1ZW9FdmlUM0dYTFI2SXErVithYSsxNHJEYWt3NytmM3ZIcEZFNk52ajNZSTVzUURmUXAxSU5PRm1HRmNPY1JXL2wxb2dMeW5BN3UvRHFBV0ZsYno5L1VzcnNleDBNc2lVYUYzUkVRd1l4a2tmakg2Mll3NS9yc2NieHpyK25lRithRWZHRjZvSVJLNmVOWjZIRDluTUx5SU5WS0NWTkt0bXFPTldJM0pFVnlQU1I1SmllSURVV3hTamRCdDE0djlZQkd5M2piLy96eWxURWJrSGxZNGNVRWZMKzBJa2E4WlluWDZPS2Y5Y2U5a3Y3Zz09"
	}

	Response-4 :
	{
		"result": false,
		"message": "Invalid Token : Token is over..."
	}