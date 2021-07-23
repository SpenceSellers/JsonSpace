namespace JsonAnalytics
{
    /// <summary>
    /// Each StructuralChar is a group of characters that can be replaced with any other character in
    /// the same StructuralChar group without breaking the JSON value.
    ///
    /// For example, a "5" as a LeadingIntegerDigit is always safe to replace with a "9"; it will generate a
    /// distinct JSON value, but will never create an un-parsable JSON value.
    ///
    /// Alternatively, a "{" as a ObjectBegin cannot be replaced with anything without breaking the JSON value.
    /// Hence, the ObjectBegin StructuralChar represents only a single possible character.
    /// </summary>
    public enum StructuralChar
    {
        Whitespace,
        ArrayBegin,
        ArrayEnd,
        ObjectBegin,
        ObjectEnd,
        KeyValueSeparator,
        Comma,
        LeadingNegative,
        LeadingIntegerDigit,
        FollowingIntegerDigit,
        DecimalSeparator,
        ScientificNotationSeparator,
        StringDelimiter,
        UnescapedStringBody,
        StringEscapeMarker,
        SingleEscapedChar,
        UnicodeEscapedChar,
        UnicodeEscapeMarker,
        OnlyZero,
        NullOne,
        NullTwo,
        NullThree,
        NullFour,
        TrueOne,
        TrueTwo,
        TrueThree,
        TrueFour,
        FalseOne,
        FalseTwo,
        FalseThree,
        FalseFour,
        FalseFive
    }
}