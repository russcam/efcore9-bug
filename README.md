# Bug in EF Core 9 with custom EncryptedAttribute

This repository is a minimal repo of a bug in EF Core 9 related to the use of an encrypted attribute to apply a convention to encrypt
attributed properties in the EF model when persisting to the DB, and to descrypt when hydrating from DB.

To repro

```sh
dotnet restore
dotnet run
```

results in the exception

```
Unhandled exception. System.NullReferenceException: Object reference not set to an instance of an object.
   at lambda_method43(Closure, QueryContext, DbDataReader, ResultContext, SingleQueryResultCoordinator)
   at Microsoft.EntityFrameworkCore.Query.Internal.SingleQueryingEnumerable`1.AsyncEnumerator.MoveNextAsync()
   at Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.ToListAsync[TSource](IQueryable`1 source, CancellationToken cancellationToken)
   at Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.ToListAsync[TSource](IQueryable`1 source, CancellationToken cancellationToken)
   at Program.<Main>$(String[] args) in C:\Users\russc\source\EFCore9Bug\EFCore9Bug\Program.cs:line 22
   at Program.<Main>(String[] args)
```

that occurs in the `.ToListAsync()` call.

It _looks_ like the problem is in the `.Select(...)` projection and specifically the projection of the property that has the Encryption value converter;
If the `Encrpyted` property is removed from the projection, it succeeds.
