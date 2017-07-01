# Sumo.Nop.MediaTools
Some tools to help with nopCommerce implementations.

## ImageExport

Exports images to disk from nopCommerce v3.9 safely.  Can be re-run as many 
times as necessary to fully export images as timeouts can occur if the Picture
table is large.  Update App.config with the DB connection string.

There are no nopCommerce dependencies in this utility.

### Parameters

```
-o Output path (i.e. D:\temp)
-s nopCommerce Store ID (required when only dealing with a mulit-store installation)
```

### How to use:

Export Only:
```
nop-tools.exe ImageExport -o="[outputPath]"
```

Export and Update DB:
```
nop-tools.exe ImageExport -o="[outputPath]" -u
```

Export, Update DB, specific store:
```
nop-tools.exe ImageExport -o="[outputPath]" -u -s=1
```