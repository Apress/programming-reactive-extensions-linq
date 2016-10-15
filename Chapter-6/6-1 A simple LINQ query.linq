<Query Kind="Statements">
  <Connection>
    <ID>b5bf0f26-eb4d-481f-aab0-d1d516538992</ID>
    <Persist>true</Persist>
    <Server>.\SQLEXPRESS</Server>
    <AttachFile>true</AttachFile>
    <UserInstance>true</UserInstance>
    <AttachFileName>L:\Downloads\northwind\NORTHWND.mdf</AttachFileName>
  </Connection>
  <Reference Relative="..\RxBookLinqpadHelper\ext\System.Reactive.dll">D:\ProgrammingRx\Demo Code For Chapters\RxBookLinqpadHelper\ext\System.Reactive.dll</Reference>
  <Reference Relative="..\RxBookLinqpadHelper\ext\Microsoft.Reactive.Testing.dll">D:\ProgrammingRx\Demo Code For Chapters\RxBookLinqpadHelper\ext\Microsoft.Reactive.Testing.dll</Reference>
  <Reference Relative="..\RxBookLinqpadHelper\RxBookLinqpadHelper\bin\Debug\RxBookLinqpadHelper.dll">D:\ProgrammingRx\Demo Code For Chapters\RxBookLinqpadHelper\RxBookLinqpadHelper\bin\Debug\RxBookLinqpadHelper.dll</Reference>
  <Namespace>System.Reactive</Namespace>
  <Namespace>System.Reactive.Linq</Namespace>
  <Namespace>Microsoft.Reactive.Testing</Namespace>
  <Namespace>RxBookLinqpadHelper</Namespace>
  <Namespace>System.Reactive.Concurrency</Namespace>
</Query>

DataContext dataContext = this;

IQueryable<Contacts> query = from Contact in dataContext.GetTable<Contacts>()
						select Contact;
						
query.Dump();