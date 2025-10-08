# PdfApi Solution

Från root-foldern kör:
`docker build -t pdfapi:[tag] -f .\PdfApi\Dockerfile .`
för att bygga en container. Exponerar port 8080 och 8081 som behöver mappas till lokala portar, antingen genom docker run cmdline argument eller genom compose.yml
