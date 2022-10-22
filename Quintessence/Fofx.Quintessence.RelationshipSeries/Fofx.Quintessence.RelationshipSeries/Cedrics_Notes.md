# Cedric's thoughts and observations

## General
I use a tab size of 4. I have not refactored all instances where the tab size is 2.

## Potential bloat

The RelationshipRevisable set of classes duplicated each other in about 95% of their code; there are 4 versions (plus others) of their very similar 'Read' method (RelationshipRevisableDateValueRequestHelper, RelationshipRevisableEnumValueRequestHelper, RelationshipRevisableNumericValueRequestHelper, RelationshipRevisableStringValueRequestHelper).

I have attempted a shared Read method to replace these 4 in 'New Classes\RelationshipArrayRequestHelper.cs'. It hasn't been debugged or tested. The combined method is still too large for one module, I would only start moving parts out once I knew it worked correctly. It may be possible to use this to replace more of the 'Read' code with more time.

## Navigation
I like things to be easy to find so have moved interfaces, abstract classes and enumerators to their own files and folders as I found each of them

I have also renamed class files to match the class names where I noticed that these were different. I have moved classes to their own files where there were multiple classes or enums sharing a file. 

A number of classes could probably be merged using generic data types - I haven't tried to implement this change.

## Scope and inheritability
I have changed the accessibility of as many classes as I could to 'internal sealed' and 'public sealed' where they are not inherited. This is to reduce visibility and sealing them improves performance.
(I realise that some classes will need to be public once their external uses are known)

## Comments
The code contained no comments. I have added a few but don't know enough about the system to write any which are meaningful.

## Null checking
I believe there should be more checking for null values before attempting to access most types of object.

## Try...catch blocks
I didn't see any try...catch blocks in use and believe that error handling could be much better. I haven't replaced the approach of throwing exceptions but would usually deal with these in a friendlier way (e.g. a popup message). 
Examples of trying to provide a friendlier error are in in RelationshipArrayRequestHelper, RelationshipDateCharacteristicRequestHelper, ArrayTypeValues and some other classes

## Data access
I noticed that Entity Framework isn't used. These days, I strongly prefer data access to use API calls and have found it easy enough to generate CRUD endpoints for every entity in a data model (or classes in a library). Note that Microsoft seems to be focusing on improving the performance of both APIs and Entity Framework and I believe that the latest versions perform really well.

## General
This is the first project I've seen in over 10 years which doesn't use Linq! (A single use of .ToArray is the only usage I found)
