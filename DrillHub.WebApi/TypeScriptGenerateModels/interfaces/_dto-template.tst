${
    using System.Text.RegularExpressions;

    Template(Settings settings) {
        settings.OutputFilenameFactory = file => 
            Regex.Replace(file.Classes.First(c => c.Name.EndsWith("Dto")).Name, "([A-Z])", "-$1").Trim('-').ToLower() + ".ts";
    }

    string ClassNameWithExtends(Class c) {
        return c.Name + (c.BaseClass != null ? " extends " + c.BaseClass.Name : "");
    }

    string InterfaceNameWithExtends(Class c) {
        return c.Name + (c.BaseClass != null ? " extends I" + c.BaseClass.Name : "");
    }

    string ExtractTypeScriptName(Property p) {
        switch (p.Type)
        {
            case "IFormFile":
                return "File";
        }

        return p.Type;
    }

    string Optional(Property p){
        return p.Type.IsNullable ? "?" : "";
    }

    string Imports(Class c) {
        var classes = new List<string>();
        var enums = new List<string>();

        foreach (var p in c.Properties) {
            if (!p.Attributes.Any(a => a == "JsonIgnore")) {
                if (p.Type.IsEnum || (p.Type.IsEnumerable && p.Type.TypeArguments.Any(g => g.IsEnum))) {
                    if (!enums.Any(e => e == p.Type.Name.Replace("[]", ""))) {
                        enums.Add(p.Type.Name.Replace("[]", ""));
                    }
                } else if (!p.Type.IsPrimitive && p.Type.Name != "IFormFile" && p.Type.Name != "any") {
                    if (!classes.Any(c => c == p.Type.Name.Replace("[]", ""))) {
                        classes.Add(p.Type.Name.Replace("[]", ""));
                    }
                }
            }
        }
        var result = "";
        foreach (var item in classes) {
            var fileName = Regex.Replace(item, "([A-Z])", "-$1").Trim('-').ToLower();
            result += "import { " + item + " } from './" + fileName + "';" + Environment.NewLine;
        }
        foreach (var item in enums) {
            var fileName = Regex.Replace(item, "([A-Z])", "-$1").Trim('-').ToLower();
            result += "import { " + item + " } from '../enums/" + fileName + "';" + Environment.NewLine;
        }
        if (c.BaseClass != null){
            var fileName = Regex.Replace(c.BaseClass, "([A-Z])", "-$1").Trim('-').ToLower();
            result += "import { I" + c.BaseClass + ", " + c.BaseClass + " } from './" + fileName + "';" + Environment.NewLine;
        }
        return result;
    }
}$Classes(*Dto)[$Imports
export interface I$InterfaceNameWithExtends {$Properties[
    $Name$Optional: $ExtractTypeScriptName;]
}

export class $ClassNameWithExtends implements I$Name {$Properties[
    $Name$Optional: $ExtractTypeScriptName;]
}]
