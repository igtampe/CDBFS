namespace Igtampe.CDBFS.Common {

    public class CdbfsFileType : Nameable {
        public static readonly CdbfsFileType[] Types = {
            new(".aac", "AAC audio","audio/aac",MIME.AAC),
            new(".abw", "AbiWord document","applicatio/x-abiword",MIME.X_ABIWORD),
            new(".arc", "Archive document","application/x-freearc",MIME.X_FREEARC),
            new(".avif", "AVIF image","image/avif",MIME.AVIF),
            new(".avi", "AVI: Audio Video Interleave","video/x-msvideo",MIME.X_MSVIDEO),
            new(".azw", "Amazon Kindle eBook format","application/vnd.amazon.ebook",MIME.VND_AMAZON_EBOOK),
            new(".bin", "Any kind of binary data","application/octet-stream",MIME.OCTET_STREAM),
            new(".bmp", "Windows OS/2 Bitmap Graphics","image/bmp",MIME.BMP),
            new(".bz", "BZip archive","application/x-bzip",MIME.X_BZIP),
            new(".bz2", "BZip2 archive","application/x-bzip2",MIME.X_BZIP2),
            new(".cda", "CD audio","application/x-cdf",MIME.X_CDF),
            new(".csh", "C-Shell script","application/x-csh",MIME.X_CSH),
            new(".css", "Cascading Style Sheets (CSS)","text/css",MIME.CSS),
            new(".csv", "Comma-separated values (CSV)","text/csv",MIME.CSV),
            new(".doc", "Microsoft Word","application/msword",MIME.MSWORD),
            new(".docx", "Microsoft Word (OpenXML)","application/vnd.openxmlformats-officedocument.wordprocessingml.document",MIME.VND_OPENXMLFORMATS_OFFICEDOCUMENT_WORDPROCESSINGML_DOCUMENT),
            new(".eot", "MS Embedded OpenType fonts","application/vnd.ms-fontobject",MIME.VND_MS_FONTOBJECT),
            new(".epub", "Electronic publication (EPUB)","application/epub+zip",MIME.EPUB_ZIP),
            new(".gz", "GZip Compressed Archive","application/gzip",MIME.GZIP),
            new(".gif", "Graphics Interchange Format (GIF)","image/gif",MIME.GIF),
            new(".htm", "HyperText Markup Language (HTML)","text/html",MIME.HTM),
            new(".html", "HyperText Markup Language (HTML)","text/html",MIME.HTML),
            new(".ico", "Icon format","image/vnd.microsoft.icon",MIME.VND_MICROSOFT_ICON),
            new(".ics", "iCalendar format","text/calendar",MIME.CALENDAR),
            new(".jar", "Java Archive (JAR)","application/java-archive",MIME.JAVA_ARCHIVE),
            new(".jpeg", ".jpg  JPEG images","image/jpeg",MIME.JPEG),
            new(".js", "JavaScript","text/javascript",MIME.JAVASCRIPT),
            new(".json", "JSON format","application/json",MIME.JSON),
            new(".jsonld", "JSON-LD format","application/ld+json",MIME.LD_JSON),
            new(".mid", "Musical Instrument Digital Interface (MIDI)", "audio/midi",MIME.MIDI),
            new(".midi", "Musical Instrument Digital Interface (MIDI)", "audio/xmidi",MIME.X_MIDI),
            new(".mp3", "MP3 audio","audio/mpeg",MIME.MPEG_AUDIO),
            new(".mp4", "MP4 video","video/mp4",MIME.MP4),
            new(".mpeg", "MPEG Video","video/mpeg",MIME.MPEG_VIDEO),
            new(".mpkg", "Apple Installer Package","application/vnd.apple.installer+xml",MIME.VND_APPLE_INSTALLER_XML),
            new(".odp", "OpenDocument presentation document","application/vnd.oasis.opendocument.presentation",MIME.VND_OASIS_OPENDOCUMENT_PRESENTATION),
            new(".ods", "OpenDocument spreadsheet document","application/vnd.oasis.opendocument.spreadsheet",MIME.VND_OASIS_OPENDOCUMENT_SPREADSHEET),
            new(".odt", "OpenDocument text document","application/vnd.oasis.opendocument.text",MIME.VND_OASIS_OPENDOCUMENT_TEXT),
            new(".ogv", "OGG video","video/ogg",MIME.OGG_VIDEO),
            new(".oga", "OGG audio","audio/ogg",MIME.OGG_AUDIO),
            new(".ogx", "OGG","application/ogg",MIME.OGG_APP),
            new(".opus", "Opus audio","audio/opus",MIME.OPUS),
            new(".otf", "OpenType font","font/otf",MIME.OTF),
            new(".png", "Portable Network Graphics","image/png",MIME.PNG),
            new(".pdf", "Adobe Portable Document Format (PDF)","application/pdf",MIME.PDF),
            new(".php", "Hypertext Preprocessor (Personal Home Page)", "application/x-httpd-php",MIME.X_HTTPD_PHP),
            new(".ppt", "Microsoft PowerPoint", "application/vnd.ms-powerpoint",MIME.VND_MS_POWERPOINT),
            new(".pptx", "Microsoft PowerPoint (OpenXML)", "application/vnd.openxmlformats-officedocument.presentationml.presentation",MIME.VND_OPENXMLFORMATS_OFFICEDOCUMENT_PRESENTATIONML_PRESENTATION),
            new(".rar", "RAR archive","application/vnd.rar",MIME.VND_RAR),
            new(".rtf", "Rich Text Format (RTF)","application/rtf",MIME.RTF),
            new(".sh", "Bourne shell script","application/x-sh",MIME.X_SH),
            new(".svg", "Scalable Vector Graphics (SVG)","image/svg+xml",MIME.SVG_XML),
            new(".swf", "Small web format (SWF) or Adobe Flash document","application/x-shockwave-flash",MIME.X_SHOCKWAVE_FLASH),
            new(".tar", "Tape Archive (TAR)","application/x-tar",MIME.X_TAR),
            new(".tif", "Tagged Image File Format (TIFF)","image/tiff",MIME.TIF),
            new(".tiff", "Tagged Image File Format (TIFF)","image/tiff",MIME.TIFF),
            new(".ts", "MPEG transport stream","video/mp2t",MIME.MP2T),
            new(".ttf", "TrueType Font","font/ttf",MIME.TTF),
            new(".txt", "Text, (generally ASCII or ISO 8859-n)","text/plain",MIME.PLAINTEXT),
            new(".vsd", "Microsoft Visio", "application/vnd.visio",MIME.VND_VISIO),
            new(".wav", "Waveform Audio Format","audio/wav",MIME.WAV),
            new(".weba", "WEBM audio","audio/webm",MIME.WEBM_AUDIO),
            new(".webm", "WEBM video","video/webm",MIME.MPEG_VIDEO),
            new(".webp", "WEBP image","image/webp",MIME.WEBP),
            new(".woff", "Web Open Font Format (WOFF)", "font/woff",MIME.WOFF),
            new(".woff2", "Web Open Font Format (WOFF)", "font/woff2",MIME.WOFF2),
            new(".xhtml", "XHTML", "application/xhtml+xml",MIME.XHTML_XML),
            new(".xls", "Microsoft Excel", "application/vnd.ms-excel",MIME.VND_MS_EXCEL),
            new(".xlsx", "Microsoft Excel (OpenXML)", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",MIME.VND_OPENXMLFORMATS_OFFICEDOCUMENT_SPREADSHEETML_SHEET),
            new(".xml", "XML", "application/xml",MIME.XML),
            new(".xul", "XUL", "application/vnd.mozilla.xul+xml",MIME.VND_MOZILLA_XUL_XML),
            new(".zip", "ZIP archive", "application/zip",MIME.ZIP),
            new(".7z", "7-zip archive", "application/x-7z-compressed",MIME.SEVENZIP)
        };

        public string Name { get; set; }
        public string MimeString { get; set; }
        public string Extension { get; set; }
        public MIME Mime { get; set; }
        private CdbfsFileType(string Extension, string Name, string MimeString, MIME Mime) {
            this.Name = Name;
            this.Extension = Extension;
            this.MimeString = MimeString;
            this.Mime = Mime;
        }

        public static CdbfsFileType FromMIMEEnum(MIME M) => Types[(int)M];
        
        public static CdbfsFileType FromExtension(string Ext) => Types.FirstOrDefault(A => A.Extension.ToLower() == Ext.ToLower()) ?? FromMIMEEnum(MIME.OCTET_STREAM);
       
    }
}
