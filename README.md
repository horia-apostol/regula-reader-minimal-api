<p align="left">
  <a href="https://www.regulaforensics.com/" target="_blank">
    <img src="https://avatars.githubusercontent.com/u/19432176?s=200&v=4" alt="Regula" height="70" />
  </a>
</p>

---

# Regula Reader Minimal API (.NET 9)

**[Regula Forensics – Official Website](https://www.regulaforensics.com/)**

This is a **minimal REST API** built with **.NET 9**, designed to expose structured data from identity documents processed by the **Regula Document Reader** via the COM interface.

---

## Regula Licensing

This project **does not include** or manage licensing for the Regula SDK. To run this project, you **must install the SDK yourself** and ensure that a valid license is provided and the COM interface is registered.

Refer to official licensing docs:  
[Regula Licensing Documentation](https://docs.regulaforensics.com/develop/doc-reader-sdk/overview/licensing/)

---

## API Documentation

Explore the full SwaggerHub spec:  
[SwaggerHub – Regula Reader Minimal API v1.0](https://app.swaggerhub.com/apis/HoriaApostol/regula-reader-minimal-api/1.0)

---

## Example Request

```
POST /api/scanner/read?fields=surname,givenNames,documentNumber&visual=true&mrz=true Accept: application/vnd.regula-reader-minimal-api.hateoas.1+json
```

## Example Response

```json
{
  "data": {
    "surname": { "visual": "DOE", "mrz": "DOE" },
    "givenNames": { "visual": "JOHN", "mrz": "JOHN" },
    "documentNumber": { "visual": "X123456", "mrz": "X123456" }
  },
  "links": {
    "portrait": "http://localhost:5000/api/scanner/image/portrait",
    "full": "http://localhost:5000/api/scanner/image/full",
    "bw": "http://localhost:5000/api/scanner/image/bw",
    "uv": "http://localhost:5000/api/scanner/image/uv"
  }
}
```

## Official Regula Documentation

- [Programmers Guide (PDF)](https://downloads.regulaforensics.com/work/SDK/doc/Programmers%20Guide%20(en).pdf)
- [RFID Programmers Guide (PDF)](https://downloads.regulaforensics.com/work/SDK/doc/Programmers%20Guide%20RFID%20(en).pdf)
- [COM Interface Documentation (PDF)](https://downloads.regulaforensics.com/work/SDK/doc/COM%20interface%20documentation.pdf)
- [Test Application Guide (PDF)](https://downloads.regulaforensics.com/work/SDK/doc/Test%20Application%20(en).pdf)

## License

This project is licensed under the **GNU General Public License v3.0** (GPLv3).  
You are free to use, modify, and distribute this project under the same terms.

See the [`LICENSE`](./LICENSE) file for full details.
