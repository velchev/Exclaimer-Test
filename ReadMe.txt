This is the project which the candidate will make alterations to.  It consists of a single class DeveloperTestImplementation which implements the interface
IDeveloperTest.


Develope Notes:

I would prefer the 2 methods RunQuestionOne and RunQuestionTwo to e in 2 separate classes/files as the dictionary is shared but is used differetnly and _dictionary.Clean(); is not intuitive why is done, 
but in the test we use one object to test call the 2 methods.

DelayedPrint is printing the output as is done in question 1 but with delay of 10 seconds. This happens till we processed all the readers

To complate it it took me few ours over the weekend. What was difficult is to get the ProcessReaderAsync workign as I started without noticing CharExtensions.cs and had to rewrite it.