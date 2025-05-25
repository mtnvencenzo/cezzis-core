namespace Cezzi.Applications.Text;

/// <summary>
/// 
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="CodePage"/> class.
/// </remarks>
/// <param name="identitifer">The identitifer.</param>
/// <param name="name">The name.</param>
/// <param name="description">The description.</param>
public class CodePage(int identitifer, string name, string description)
{
    /// <summary>Gets the identifier.</summary>
    /// <value>The identifier.</value>
    public int Identifier { get; private set; } = identitifer;

    /// <summary>Gets the name.</summary>
    /// <value>The name.</value>
    public string Name { get; private set; } = name;

    /// <summary>Gets the description.</summary>
    /// <value>The description.</value>
    public string Description { get; private set; } = description;

    /// <summary>
    /// Returns a <see cref="string" /> that represents this instance.
    /// </summary>
    /// <returns>A <see cref="string" /> that represents this instance.</returns>
    public override string ToString() => this.Name;

    /// <summary>Returns a hash code for this instance.</summary>
    /// <returns>
    /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
    /// </returns>
    public override int GetHashCode() => this.Name.GetHashCode();

    #region Code Pages

    /// <summary>Gets the ib M037.</summary>
    /// <value>The ib M037.</value>
    public static CodePage IBM037 => new(37, "IBM037", "IBM EBCDIC US-Canada");
    /// <summary>Gets the ib M437.</summary>
    /// <value>The ib M437.</value>
    public static CodePage IBM437 => new(437, "IBM437", "OEM United States");
    /// <summary>Gets the ib M500.</summary>
    /// <value>The ib M500.</value>
    public static CodePage IBM500 => new(500, "IBM500", "IBM EBCDIC International");
    /// <summary>Gets the asm o708.</summary>
    /// <value>The asm o708.</value>
    public static CodePage ASMO708 => new(708, "ASMO-708", "Arabic (ASMO 708)");
    /// <summary>Gets the do S720.</summary>
    /// <value>The do S720.</value>
    public static CodePage DOS720 => new(720, "DOS-720", "Arabic (Transparent ASMO); Arabic (DOS)");
    /// <summary>Gets the ibm737.</summary>
    /// <value>The ibm737.</value>
    public static CodePage ibm737 => new(737, "ibm737", "OEM Greek (formerly 437G); Greek (DOS)");
    /// <summary>Gets the ibm775.</summary>
    /// <value>The ibm775.</value>
    public static CodePage ibm775 => new(775, "ibm775", "OEM Baltic; Baltic (DOS)");
    /// <summary>Gets the ibm850.</summary>
    /// <value>The ibm850.</value>
    public static CodePage ibm850 => new(850, "ibm850", "OEM Multilingual Latin 1; Western European (DOS)");
    /// <summary>Gets the ibm852.</summary>
    /// <value>The ibm852.</value>
    public static CodePage ibm852 => new(852, "ibm852", "OEM Latin 2; Central European (DOS)");
    /// <summary>Gets the ib M855.</summary>
    /// <value>The ib M855.</value>
    public static CodePage IBM855 => new(855, "IBM855", "OEM Cyrillic (primarily Russian)");
    /// <summary>Gets the ibm857.</summary>
    /// <value>The ibm857.</value>
    public static CodePage ibm857 => new(857, "ibm857", "OEM Turkish; Turkish (DOS)");
    /// <summary>Gets the ib M00858.</summary>
    /// <value>The ib M00858.</value>
    public static CodePage IBM00858 => new(858, "IBM00858", "OEM Multilingual Latin 1 + Euro symbol");
    /// <summary>Gets the ib M860.</summary>
    /// <value>The ib M860.</value>
    public static CodePage IBM860 => new(860, "IBM860", "OEM Portuguese; Portuguese (DOS)");
    /// <summary>Gets the ibm861.</summary>
    /// <value>The ibm861.</value>
    public static CodePage ibm861 => new(861, "ibm861", "OEM Icelandic; Icelandic (DOS)");
    /// <summary>Gets the do S862.</summary>
    /// <value>The do S862.</value>
    public static CodePage DOS862 => new(862, "DOS-862", "OEM Hebrew; Hebrew (DOS)");
    /// <summary>Gets the ib M863.</summary>
    /// <value>The ib M863.</value>
    public static CodePage IBM863 => new(863, "IBM863", "OEM French Canadian; French Canadian (DOS)");
    /// <summary>Gets the ib M864.</summary>
    /// <value>The ib M864.</value>
    public static CodePage IBM864 => new(864, "IBM864", "OEM Arabic; Arabic (864)");
    /// <summary>Gets the ib M865.</summary>
    /// <value>The ib M865.</value>
    public static CodePage IBM865 => new(865, "IBM865", "OEM Nordic; Nordic (DOS)");
    /// <summary>Gets the CP866.</summary>
    /// <value>The CP866.</value>
    public static CodePage cp866 => new(866, "cp866", "OEM Russian; Cyrillic (DOS)");
    /// <summary>Gets the ibm869.</summary>
    /// <value>The ibm869.</value>
    public static CodePage ibm869 => new(869, "ibm869", "OEM Modern Greek; Greek, Modern (DOS)");
    /// <summary>Gets the ib M870.</summary>
    /// <value>The ib M870.</value>
    public static CodePage IBM870 => new(870, "IBM870", "IBM EBCDIC Multilingual/ROECE (Latin 2); IBM EBCDIC Multilingual Latin 2");
    /// <summary>Gets the windows874.</summary>
    /// <value>The windows874.</value>
    public static CodePage windows874 => new(874, "windows-874", "ANSI/OEM Thai (ISO 8859-11); Thai (Windows)");
    /// <summary>Gets the CP875.</summary>
    /// <value>The CP875.</value>
    public static CodePage cp875 => new(875, "cp875", "IBM EBCDIC Greek Modern");
    /// <summary>Gets the shift jis.</summary>
    /// <value>The shift jis.</value>
    public static CodePage shift_jis => new(932, "shift_jis", "ANSI/OEM Japanese; Japanese (Shift-JIS)");
    /// <summary>Gets the GB2312.</summary>
    /// <value>The GB2312.</value>
    public static CodePage gb2312 => new(936, "gb2312", "ANSI/OEM Simplified Chinese (PRC, Singapore); Chinese Simplified (GB2312)");
    /// <summary>Gets the ks c 56011987.</summary>
    /// <value>The ks c 56011987.</value>
    public static CodePage ks_c_56011987 => new(949, "ks_c_5601-1987", "ANSI/OEM Korean (Unified Hangul Code)");
    /// <summary>Gets the big5.</summary>
    /// <value>The big5.</value>
    public static CodePage big5 => new(950, "big5", "ANSI/OEM Traditional Chinese (Taiwan; Hong Kong SAR, PRC); Chinese Traditional (Big5)");
    /// <summary>Gets the ib M1026.</summary>
    /// <value>The ib M1026.</value>
    public static CodePage IBM1026 => new(1026, "IBM1026", "IBM EBCDIC Turkish (Latin 5)");
    /// <summary>Gets the ib M01047.</summary>
    /// <value>The ib M01047.</value>
    public static CodePage IBM01047 => new(1047, "IBM01047", "IBM EBCDIC Latin 1/Open System");
    /// <summary>Gets the ib M01140.</summary>
    /// <value>The ib M01140.</value>
    public static CodePage IBM01140 => new(1140, "IBM01140", "IBM EBCDIC US-Canada (037 + Euro symbol); IBM EBCDIC (US-Canada-Euro)");
    /// <summary>Gets the ib M01141.</summary>
    /// <value>The ib M01141.</value>
    public static CodePage IBM01141 => new(1141, "IBM01141", "IBM EBCDIC Germany (20273 + Euro symbol); IBM EBCDIC (Germany-Euro)");
    /// <summary>Gets the ib M01142.</summary>
    /// <value>The ib M01142.</value>
    public static CodePage IBM01142 => new(1142, "IBM01142", "IBM EBCDIC Denmark-Norway (20277 + Euro symbol); IBM EBCDIC (Denmark-Norway-Euro)");
    /// <summary>Gets the ib M01143.</summary>
    /// <value>The ib M01143.</value>
    public static CodePage IBM01143 => new(1143, "IBM01143", "IBM EBCDIC Finland-Sweden (20278 + Euro symbol); IBM EBCDIC (Finland-Sweden-Euro)");
    /// <summary>Gets the ib M01144.</summary>
    /// <value>The ib M01144.</value>
    public static CodePage IBM01144 => new(1144, "IBM01144", "IBM EBCDIC Italy (20280 + Euro symbol); IBM EBCDIC (Italy-Euro)");
    /// <summary>Gets the ib M01145.</summary>
    /// <value>The ib M01145.</value>
    public static CodePage IBM01145 => new(1145, "IBM01145", "IBM EBCDIC Latin America-Spain (20284 + Euro symbol); IBM EBCDIC (Spain-Euro)");
    /// <summary>Gets the ib M01146.</summary>
    /// <value>The ib M01146.</value>
    public static CodePage IBM01146 => new(1146, "IBM01146", "IBM EBCDIC United Kingdom (20285 + Euro symbol); IBM EBCDIC (UK-Euro)");
    /// <summary>Gets the ib M01147.</summary>
    /// <value>The ib M01147.</value>
    public static CodePage IBM01147 => new(1147, "IBM01147", "IBM EBCDIC France (20297 + Euro symbol); IBM EBCDIC (France-Euro)");
    /// <summary>Gets the ib M01148.</summary>
    /// <value>The ib M01148.</value>
    public static CodePage IBM01148 => new(1148, "IBM01148", "IBM EBCDIC International (500 + Euro symbol); IBM EBCDIC (International-Euro)");
    /// <summary>Gets the ib M01149.</summary>
    /// <value>The ib M01149.</value>
    public static CodePage IBM01149 => new(1149, "IBM01149", "IBM EBCDIC Icelandic (20871 + Euro symbol); IBM EBCDIC (Icelandic-Euro)");
    /// <summary>Gets the UTF16.</summary>
    /// <value>The UTF16.</value>
    public static CodePage utf16 => new(1200, "utf-16", "Unicode UTF-16, little endian byte order (BMP of ISO 10646); available only to managed applications");
    /// <summary>Gets the unicode fffe.</summary>
    /// <value>The unicode fffe.</value>
    public static CodePage unicodeFFFE => new(1201, "unicodeFFFE", "Unicode UTF-16, big endian byte order; available only to managed applications");
    /// <summary>Gets the windows1250.</summary>
    /// <value>The windows1250.</value>
    public static CodePage windows1250 => new(1250, "windows-1250", "ANSI Central European; Central European (Windows)");
    /// <summary>Gets the windows1251.</summary>
    /// <value>The windows1251.</value>
    public static CodePage windows1251 => new(1251, "windows-1251", "ANSI Cyrillic; Cyrillic (Windows)");
    /// <summary>Gets the windows1252.</summary>
    /// <value>The windows1252.</value>
    public static CodePage windows1252 => new(1252, "windows-1252", "ANSI Latin 1; Western European (Windows)");
    /// <summary>Gets the windows1253.</summary>
    /// <value>The windows1253.</value>
    public static CodePage windows1253 => new(1253, "windows-1253", "ANSI Greek; Greek (Windows)");
    /// <summary>Gets the windows1254.</summary>
    /// <value>The windows1254.</value>
    public static CodePage windows1254 => new(1254, "windows-1254", "ANSI Turkish; Turkish (Windows)");
    /// <summary>Gets the windows1255.</summary>
    /// <value>The windows1255.</value>
    public static CodePage windows1255 => new(1255, "windows-1255", "ANSI Hebrew; Hebrew (Windows)");
    /// <summary>Gets the windows1256.</summary>
    /// <value>The windows1256.</value>
    public static CodePage windows1256 => new(1256, "windows-1256", "ANSI Arabic; Arabic (Windows)");
    /// <summary>Gets the windows1257.</summary>
    /// <value>The windows1257.</value>
    public static CodePage windows1257 => new(1257, "windows-1257", "ANSI Baltic; Baltic (Windows)");
    /// <summary>Gets the windows1258.</summary>
    /// <value>The windows1258.</value>
    public static CodePage windows1258 => new(1258, "windows-1258", "ANSI/OEM Vietnamese; Vietnamese (Windows)");
    /// <summary>Gets the johab.</summary>
    /// <value>The johab.</value>
    public static CodePage Johab => new(1361, "Johab", "Korean (Johab)");
    /// <summary>Gets the macintosh.</summary>
    /// <value>The macintosh.</value>
    public static CodePage macintosh => new(10000, "macintosh", "MAC Roman; Western European (Mac)");
    /// <summary>Gets the xmacjapanese.</summary>
    /// <value>The xmacjapanese.</value>
    public static CodePage xmacjapanese => new(10001, "x-mac-japanese", "Japanese (Mac)");
    /// <summary>Gets the xmacchinesetrad.</summary>
    /// <value>The xmacchinesetrad.</value>
    public static CodePage xmacchinesetrad => new(10002, "x-mac-chinesetrad", "MAC Traditional Chinese (Big5); Chinese Traditional (Mac)");
    /// <summary>Gets the xmackorean.</summary>
    /// <value>The xmackorean.</value>
    public static CodePage xmackorean => new(10003, "x-mac-korean", "Korean (Mac)");
    /// <summary>Gets the xmacarabic.</summary>
    /// <value>The xmacarabic.</value>
    public static CodePage xmacarabic => new(10004, "x-mac-arabic", "Arabic (Mac)");
    /// <summary>Gets the xmachebrew.</summary>
    /// <value>The xmachebrew.</value>
    public static CodePage xmachebrew => new(10005, "x-mac-hebrew", "Hebrew (Mac)");
    /// <summary>Gets the xmacgreek.</summary>
    /// <value>The xmacgreek.</value>
    public static CodePage xmacgreek => new(10006, "x-mac-greek", "Greek (Mac)");
    /// <summary>Gets the xmaccyrillic.</summary>
    /// <value>The xmaccyrillic.</value>
    public static CodePage xmaccyrillic => new(10007, "x-mac-cyrillic", "Cyrillic (Mac)");
    /// <summary>Gets the xmacchinesesimp.</summary>
    /// <value>The xmacchinesesimp.</value>
    public static CodePage xmacchinesesimp => new(10008, "x-mac-chinesesimp", "MAC Simplified Chinese (GB 2312); Chinese Simplified (Mac)");
    /// <summary>Gets the xmacromanian.</summary>
    /// <value>The xmacromanian.</value>
    public static CodePage xmacromanian => new(10010, "x-mac-romanian", "Romanian (Mac)");
    /// <summary>Gets the xmacukrainian.</summary>
    /// <value>The xmacukrainian.</value>
    public static CodePage xmacukrainian => new(10017, "x-mac-ukrainian", "Ukrainian (Mac)");
    /// <summary>Gets the xmacthai.</summary>
    /// <value>The xmacthai.</value>
    public static CodePage xmacthai => new(10021, "x-mac-thai", "Thai (Mac)");
    /// <summary>Gets the xmacce.</summary>
    /// <value>The xmacce.</value>
    public static CodePage xmacce => new(10029, "x-mac-ce", "MAC Latin 2; Central European (Mac)");
    /// <summary>Gets the xmacicelandic.</summary>
    /// <value>The xmacicelandic.</value>
    public static CodePage xmacicelandic => new(10079, "x-mac-icelandic", "Icelandic (Mac)");
    /// <summary>Gets the xmacturkish.</summary>
    /// <value>The xmacturkish.</value>
    public static CodePage xmacturkish => new(10081, "x-mac-turkish", "Turkish (Mac)");
    /// <summary>Gets the xmaccroatian.</summary>
    /// <value>The xmaccroatian.</value>
    public static CodePage xmaccroatian => new(10082, "x-mac-croatian", "Croatian (Mac)");
    /// <summary>Gets the utf32.</summary>
    /// <value>The utf32.</value>
    public static CodePage utf32 => new(12000, "utf-32", "Unicode UTF-32, little endian byte order; available only to managed applications");
    /// <summary>Gets the utf32 be.</summary>
    /// <value>The utf32 be.</value>
    public static CodePage utf32BE => new(12001, "utf-32BE", "Unicode UTF-32, big endian byte order; available only to managed applications");
    /// <summary>Gets the x chinese CNS.</summary>
    /// <value>The x chinese CNS.</value>
    public static CodePage xChinese_CNS => new(20000, "x-Chinese_CNS", "CNS Taiwan; Chinese Traditional (CNS)");
    /// <summary>Gets the XCP20001.</summary>
    /// <value>The XCP20001.</value>
    public static CodePage xcp20001 => new(20001, "x-cp20001", "TCA Taiwan");
    /// <summary>Gets the x chinese eten.</summary>
    /// <value>The x chinese eten.</value>
    public static CodePage x_ChineseEten => new(20002, "x_Chinese-Eten", "Eten Taiwan; Chinese Traditional (Eten)");
    /// <summary>Gets the XCP20003.</summary>
    /// <value>The XCP20003.</value>
    public static CodePage xcp20003 => new(20003, "x-cp20003", "IBM5550 Taiwan");
    /// <summary>Gets the XCP20004.</summary>
    /// <value>The XCP20004.</value>
    public static CodePage xcp20004 => new(20004, "x-cp20004", "TeleText Taiwan");
    /// <summary>Gets the XCP20005.</summary>
    /// <value>The XCP20005.</value>
    public static CodePage xcp20005 => new(20005, "x-cp20005", "Wang Taiwan");
    /// <summary>Gets the x i a5.</summary>
    /// <value>The x i a5.</value>
    public static CodePage xIA5 => new(20105, "x-IA5", "IA5 (IRV International Alphabet No. 5, 7-bit); Western European (IA5)");
    /// <summary>Gets the x i a5 german.</summary>
    /// <value>The x i a5 german.</value>
    public static CodePage xIA5German => new(20106, "x-IA5-German", "IA5 German (7-bit)");
    /// <summary>Gets the x i a5 swedish.</summary>
    /// <value>The x i a5 swedish.</value>
    public static CodePage xIA5Swedish => new(20107, "x-IA5-Swedish", "IA5 Swedish (7-bit)");
    /// <summary>Gets the x i a5 norwegian.</summary>
    /// <value>The x i a5 norwegian.</value>
    public static CodePage xIA5Norwegian => new(20108, "x-IA5-Norwegian", "IA5 Norwegian (7-bit)");
    /// <summary>Gets the usascii.</summary>
    /// <value>The usascii.</value>
    public static CodePage usascii => new(20127, "us-ascii", "US-ASCII (7-bit)");
    /// <summary>Gets the XCP20261.</summary>
    /// <value>The XCP20261.</value>
    public static CodePage xcp20261 => new(20261, "x-cp20261", "T.61");
    /// <summary>Gets the XCP20269.</summary>
    /// <value>The XCP20269.</value>
    public static CodePage xcp20269 => new(20269, "x-cp20269", "ISO 6937 Non-Spacing Accent");
    /// <summary>Gets the ib M273.</summary>
    /// <value>The ib M273.</value>
    public static CodePage IBM273 => new(20273, "IBM273", "IBM EBCDIC Germany");
    /// <summary>Gets the ib M277.</summary>
    /// <value>The ib M277.</value>
    public static CodePage IBM277 => new(20277, "IBM277", "IBM EBCDIC Denmark-Norway");
    /// <summary>Gets the ib M278.</summary>
    /// <value>The ib M278.</value>
    public static CodePage IBM278 => new(20278, "IBM278", "IBM EBCDIC Finland-Sweden");
    /// <summary>Gets the ib M280.</summary>
    /// <value>The ib M280.</value>
    public static CodePage IBM280 => new(20280, "IBM280", "IBM EBCDIC Italy");
    /// <summary>Gets the ib M284.</summary>
    /// <value>The ib M284.</value>
    public static CodePage IBM284 => new(20284, "IBM284", "IBM EBCDIC Latin America-Spain");
    /// <summary>Gets the ib M285.</summary>
    /// <value>The ib M285.</value>
    public static CodePage IBM285 => new(20285, "IBM285", "IBM EBCDIC United Kingdom");
    /// <summary>Gets the ib M290.</summary>
    /// <value>The ib M290.</value>
    public static CodePage IBM290 => new(20290, "IBM290", "IBM EBCDIC Japanese Katakana Extended");
    /// <summary>Gets the ib M297.</summary>
    /// <value>The ib M297.</value>
    public static CodePage IBM297 => new(20297, "IBM297", "IBM EBCDIC France");
    /// <summary>Gets the ib M420.</summary>
    /// <value>The ib M420.</value>
    public static CodePage IBM420 => new(20420, "IBM420", "IBM EBCDIC Arabic");
    /// <summary>Gets the ib M423.</summary>
    /// <value>The ib M423.</value>
    public static CodePage IBM423 => new(20423, "IBM423", "IBM EBCDIC Greek");
    /// <summary>Gets the ib M424.</summary>
    /// <value>The ib M424.</value>
    public static CodePage IBM424 => new(20424, "IBM424", "IBM EBCDIC Hebrew");
    /// <summary>Gets the x EBCDIC korean extended.</summary>
    /// <value>The x EBCDIC korean extended.</value>
    public static CodePage xEBCDICKoreanExtended => new(20833, "x-EBCDIC-KoreanExtended", "IBM EBCDIC Korean Extended");
    /// <summary>Gets the ibm thai.</summary>
    /// <value>The ibm thai.</value>
    public static CodePage IBMThai => new(20838, "IBM-Thai", "IBM EBCDIC Thai");
    /// <summary>Gets the koi8r.</summary>
    /// <value>The koi8r.</value>
    public static CodePage koi8r => new(20866, "koi8-r", "Russian (KOI8-R); Cyrillic (KOI8-R)");
    /// <summary>Gets the ib M871.</summary>
    /// <value>The ib M871.</value>
    public static CodePage IBM871 => new(20871, "IBM871", "IBM EBCDIC Icelandic");
    /// <summary>Gets the ib M880.</summary>
    /// <value>The ib M880.</value>
    public static CodePage IBM880 => new(20880, "IBM880", "IBM EBCDIC Cyrillic Russian");
    /// <summary>Gets the ib M905.</summary>
    /// <value>The ib M905.</value>
    public static CodePage IBM905 => new(20905, "IBM905", "IBM EBCDIC Turkish");
    /// <summary>Gets the ib M00924.</summary>
    /// <value>The ib M00924.</value>
    public static CodePage IBM00924 => new(20924, "IBM00924", "IBM EBCDIC Latin 1/Open System (1047 + Euro symbol)");
    /// <summary>Gets the eucjp.</summary>
    /// <value>The eucjp.</value>
    public static CodePage EUCJP => new(20932, "EUC-JP", "Japanese (JIS 0208-1990 and 0212-1990)");
    /// <summary>Gets the XCP20936.</summary>
    /// <value>The XCP20936.</value>
    public static CodePage xcp20936 => new(20936, "x-cp20936", "Simplified Chinese (GB2312); Chinese Simplified (GB2312-80)");
    /// <summary>Gets the XCP20949.</summary>
    /// <value>The XCP20949.</value>
    public static CodePage xcp20949 => new(20949, "x-cp20949", "Korean Wansung");
    /// <summary>Gets the CP1025.</summary>
    /// <value>The CP1025.</value>
    public static CodePage cp1025 => new(21025, "cp1025", "IBM EBCDIC Cyrillic Serbian-Bulgarian");
    /// <summary>Gets the koi8u.</summary>
    /// <value>The koi8u.</value>
    public static CodePage koi8u => new(21866, "koi8-u", "Ukrainian (KOI8-U); Cyrillic (KOI8-U)");
    /// <summary>Gets the iso88591.</summary>
    /// <value>The iso88591.</value>
    public static CodePage iso88591 => new(28591, "iso-8859-1", "ISO 8859-1 Latin 1; Western European (ISO)");
    /// <summary>Gets the iso88592.</summary>
    /// <value>The iso88592.</value>
    public static CodePage iso88592 => new(28592, "iso-8859-2", "ISO 8859-2 Central European; Central European (ISO)");
    /// <summary>Gets the iso88593.</summary>
    /// <value>The iso88593.</value>
    public static CodePage iso88593 => new(28593, "iso-8859-3", "ISO 8859-3 Latin 3");
    /// <summary>Gets the iso88594.</summary>
    /// <value>The iso88594.</value>
    public static CodePage iso88594 => new(28594, "iso-8859-4", "ISO 8859-4 Baltic");
    /// <summary>Gets the iso88595.</summary>
    /// <value>The iso88595.</value>
    public static CodePage iso88595 => new(28595, "iso-8859-5", "ISO 8859-5 Cyrillic");
    /// <summary>Gets the iso88596.</summary>
    /// <value>The iso88596.</value>
    public static CodePage iso88596 => new(28596, "iso-8859-6", "ISO 8859-6 Arabic");
    /// <summary>Gets the iso88597.</summary>
    /// <value>The iso88597.</value>
    public static CodePage iso88597 => new(28597, "iso-8859-7", "ISO 8859-7 Greek");
    /// <summary>Gets the iso88598.</summary>
    /// <value>The iso88598.</value>
    public static CodePage iso88598 => new(28598, "iso-8859-8", "ISO 8859-8 Hebrew; Hebrew (ISO-Visual)");
    /// <summary>Gets the iso88599.</summary>
    /// <value>The iso88599.</value>
    public static CodePage iso88599 => new(28599, "iso-8859-9", "ISO 8859-9 Turkish");
    /// <summary>Gets the iso885913.</summary>
    /// <value>The iso885913.</value>
    public static CodePage iso885913 => new(28603, "iso-8859-13", "ISO 8859-13 Estonian");
    /// <summary>Gets the iso885915.</summary>
    /// <value>The iso885915.</value>
    public static CodePage iso885915 => new(28605, "iso-8859-15", "ISO 8859-15 Latin 9");
    /// <summary>Gets the x europa.</summary>
    /// <value>The x europa.</value>
    public static CodePage xEuropa => new(29001, "x-Europa", "Europa 3");
    /// <summary>Gets the iso88598i.</summary>
    /// <value>The iso88598i.</value>
    public static CodePage iso88598i => new(38598, "iso-8859-8-i", "ISO 8859-8 Hebrew; Hebrew (ISO-Logical)");
    /// <summary>Gets the iso2022jp.</summary>
    /// <value>The iso2022jp.</value>
    public static CodePage iso2022jp => new(50220, "iso-2022-jp", "ISO 2022 Japanese with no halfwidth Katakana; Japanese (JIS)");
    /// <summary>Gets the cs is o2022 jp.</summary>
    /// <value>The cs is o2022 jp.</value>
    public static CodePage csISO2022JP => new(50221, "csISO2022JP", "ISO 2022 Japanese with halfwidth Katakana; Japanese (JIS-Allow 1 byte Kana)");
    /// <summary>Gets the iso2022kr.</summary>
    /// <value>The iso2022kr.</value>
    public static CodePage iso2022kr => new(50225, "iso-2022-kr", "ISO 2022 Korean");
    /// <summary>Gets the XCP50227.</summary>
    /// <value>The XCP50227.</value>
    public static CodePage xcp50227 => new(50227, "x-cp50227", "ISO 2022 Simplified Chinese; Chinese Simplified (ISO 2022)");
    /// <summary>Gets the eucjp.</summary>
    /// <value>The eucjp.</value>
    public static CodePage eucjp => new(51932, "euc-jp", "EUC Japanese");
    /// <summary>Gets the euccn.</summary>
    /// <value>The euccn.</value>
    public static CodePage EUCCN => new(51936, "EUC-CN", "EUC Simplified Chinese; Chinese Simplified (EUC)");
    /// <summary>Gets the euckr.</summary>
    /// <value>The euckr.</value>
    public static CodePage euckr => new(51949, "euc-kr", "EUC Korean");
    /// <summary>Gets the HZGB2312.</summary>
    /// <value>The HZGB2312.</value>
    public static CodePage hzgb2312 => new(52936, "hz-gb-2312", "HZ-GB2312 Simplified Chinese; Chinese Simplified (HZ)");
    /// <summary>Gets the g B18030.</summary>
    /// <value>The g B18030.</value>
    public static CodePage GB18030 => new(54936, "GB18030", "Windows XP and later: GB18030 Simplified Chinese (4 byte); Chinese Simplified (GB18030)");
    /// <summary>Gets the xisciide.</summary>
    /// <value>The xisciide.</value>
    public static CodePage xisciide => new(57002, "x-iscii-de", "ISCII Devanagari");
    /// <summary>Gets the xisciibe.</summary>
    /// <value>The xisciibe.</value>
    public static CodePage xisciibe => new(57003, "x-iscii-be", "ISCII Bangla");
    /// <summary>Gets the xisciita.</summary>
    /// <value>The xisciita.</value>
    public static CodePage xisciita => new(57004, "x-iscii-ta", "ISCII Tamil");
    /// <summary>Gets the xisciite.</summary>
    /// <value>The xisciite.</value>
    public static CodePage xisciite => new(57005, "x-iscii-te", "ISCII Telugu");
    /// <summary>Gets the xisciias.</summary>
    /// <value>The xisciias.</value>
    public static CodePage xisciias => new(57006, "x-iscii-as", "ISCII Assamese");
    /// <summary>Gets the xisciior.</summary>
    /// <value>The xisciior.</value>
    public static CodePage xisciior => new(57007, "x-iscii-or", "ISCII Odia");
    /// <summary>Gets the xisciika.</summary>
    /// <value>The xisciika.</value>
    public static CodePage xisciika => new(57008, "x-iscii-ka", "ISCII Kannada");
    /// <summary>Gets the xisciima.</summary>
    /// <value>The xisciima.</value>
    public static CodePage xisciima => new(57009, "x-iscii-ma", "ISCII Malayalam");
    /// <summary>Gets the xisciigu.</summary>
    /// <value>The xisciigu.</value>
    public static CodePage xisciigu => new(57010, "x-iscii-gu", "ISCII Gujarati");
    /// <summary>Gets the xisciipa.</summary>
    /// <value>The xisciipa.</value>
    public static CodePage xisciipa => new(57011, "x-iscii-pa", "ISCII Punjabi");
    /// <summary>Gets the utf7.</summary>
    /// <value>The utf7.</value>
    public static CodePage utf7 => new(65000, "utf-7", "Unicode (UTF-7)");
    /// <summary>Gets the UTF8.</summary>
    /// <value>The UTF8.</value>
    public static CodePage utf8 => new(65001, "utf-8", "Unicode (UTF-8)");

    #endregion
}
