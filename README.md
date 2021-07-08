# Megamind-HtmlHelper

##How to use Select
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
