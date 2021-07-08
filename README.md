# Megamind-HtmlHelper

## How to use Select
```
Add Select cdn to layout.cshtml
<link href="https://cdn.jsdelivr.net/npm/select2@4.1.0-rc.0/dist/css/select2.min.css" rel="stylesheet" />
<script src="https://cdn.jsdelivr.net/npm/select2@4.1.0-rc.0/dist/js/select2.min.js"></script>
```

```
@{
   Html.Megamind().Select(settings =>
    {
        settings.Name = "selectexample";
        settings.ServerSide = true;
        settings.DetailSetting.ApiRoute = Url.Action("API-EndPoint");
        settings.DisplayMember = "Name"; //Properties you want to display ex: Name
        settings.ValueMember = "Id"; //value you want to get after selecting
        
    });
 }
```
## How to use Data Table
```sh
In Datatable you need to Create PartialViewResult in Controller
and Render it to the main page
@Html.Action("YourPartialViewResultForDataTable");
```

```
@{
Html.Megamind().DataTable(settings =>
    {
        settings.Name = "example";
        settings.ServerSide = true ;
        settings.DetailSetting.CallbackRoute = Url.Action("ListDataTablePartial");
        settings.DetailSetting.ApiRoute = Url.Action("GetTestData");
        settings.DetailSetting.UpdateCallbackRoute= Url.Action("GetTestData");
        settings.DetailSetting.DeleteCallbackRoute = Url.Action("Delete");

        settings.Properties.EnableDelete = true;
        settings.Properties.EnableEdit = true;

        settings.Properties.CustomCss = "custom-css-from-datahelper";
        settings.Properties.CustomStyle = "custom-style-from-datahelper";
        settings.Properties.HeaderProperties.CustomCss = "bg-blue";
        settings.Columns.Add(col =>
        {
            col.Name = "FirstName";
            col.Caption = "First Name";
            col.Properties.AddClass("text-center");
        });

        settings.Columns.Add(col =>
        {
            col.Name = "LastName";
        });
        settings.Columns.Add(col =>
        {
            col.Name = nameof(Test.BirthDate);
            col.Caption = "Birth Date";
            col.Properties.DisplayFormatString="MM/DD/YYYY";
        });
        settings.SetTemplateContent<Test>((content) =>
        {

        });
    });
}
```
