Following are some points like to share- 
1. There are total 8 projects in the solution. 
	Teoco.App
	Teoco.App.Tests
	Teoco.Common
	Teoco.Common.Tests
	Teoco.Interface
	Teoco.Interface.Tests
	Teoco.Parser
	Teoco.Parser.Tests
2. Each project has a test project associated with it. 
3. Some of the test projects are added for completeness. 
4. Teoco.App is the entry point. 
5. Added sufficient comments in each methods to explain its job.
6. There are total 34 test cases overall
7. Used Moq for mocking

Note: From the requirement came to understand that the time format is ISO 8601 Duration format(P[n]Y[n]M[n]DT[n]H[n]M[n]S). 
But in this current requirement concentration is mainly on Year, Month, Week and Days. However as per the ISO format standard,
if M comes after D, that it need to be considered as Minuites. We have data where in the sample like 1d3m, which is 1 day and 3 minuites.
Os, I considered an enhance requirement of considering Year, Month, Week, Day and Minuite and Omitted Hour, Second.
