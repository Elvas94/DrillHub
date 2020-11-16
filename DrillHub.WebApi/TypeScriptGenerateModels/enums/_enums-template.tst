${
    Template(Settings settings) {
        settings.OutputFilenameFactory = file => 
            System.Text.RegularExpressions.Regex.Replace(file.Enums.First().Name, "([A-Z])", "-$1").Trim('-').ToLower() + ".ts";
    }

    bool AcceptableEnum(Enum enumItem)
    {
        return enumItem.Namespace.Contains(".Model.") || enumItem.Namespace.Contains(".Web.");
    }
}$Enums(e => AcceptableEnum(e))[export enum $Name {$Values[
    $Name = $Value][,]
}
]