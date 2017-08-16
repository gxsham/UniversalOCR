# UniversalOCR

UniversalOCR is an application that combines two OCR libraries: [tesseract](https://github.com/tesseract-ocr) and [tikaondotnet](https://kevm.github.io/tikaondotnet/) to be able to process more formats at once. You should not worry about your personal files because they are all deleted after processing, without any logging or saving. You can check that in code.

Running application can be found here: **[Text Extractor](http://text-extractor.azurewebsites.net/)**

## API usage
Supported Formats: 
```
.txt .pdf .doc .docx .xls .xlsx .ppt pptx .rtf .bmp .png .png .jfif .jpeg .tiff
```

If you want to process a image file you need to select one of the available languages for a better result(default is *ron*):
```
afr , amh , ara , ara , asm , aze , bel , ben , bod , bos , bul , cat , ceb , ces , chi , chr , cym , dan , deu , dzo , ell , eng , enm , epo , equ , est , eus , fas , fin , fra , frk , frm , gle , glg , grc , guj , hat , heb , hin , hrv , hun , iku , ind , isl , ita , jav , jpn , kan , kat , kaz , khm , kir , kor , kur , lao , lat , lav , lit , mal , mar , mkd , mlt , msa , mya , nep , nld , nor , ori , osd , pan , pol , por , pus , ron , rus , san , sin , slk , slv , spa , spa_old , sqi , srp , swa , swe , syr , tam , tel , tgk , tgl , tha , tir , tur , uig , ukr , urd , uzb , vie , yid
```

POST : `http://text-extractor.azurewebsites.net/api/File/process`

Sending parameters:
* File  - type `file` - file to be processed
* Language - type `text` - selected language (required for images)

Result:
* Success - type `bool` 
* Text - type `text` - if Success is true, then Text contains the processing results, otherwise it contains the Error message
* ExecutionTime - type `long` - processing time in milliseconds

## Code usage

**!Important**

If you clone this code and want to run it you need to add a folder called "tessdata" in the UniversalOCR Project. There you need to add one or more language data files from [tesseract-ocr github repository](https://github.com/tesseract-ocr/tessdata/tree/3.04.00). Don't forget to set *Copy To Output Directory* property to **Copy if newer**.
