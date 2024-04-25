# Shortener Service

This is a URL shortener service implemented in ASP.NET Core and MongoDB. It allows users to shorten long URLs into shorter, more manageable ones.

## Features

- Shorten long URLs into unique, short codes
- Redirect users from short URLs to original long URLs
- MongoDB database for persisting data
- RESTful API for easy integration with other applications

## Technologies Used

- ASP.NET Core
- MongoDB
- C#
- Minimal APIs
- InMemory Cache

## Installation

1. Clone the repository:

```bash
git clone https://github.com/thisisnabi/Shortener.git
```

## Give a Star! ⭐
If you find this project helpful or interesting, please consider giving it a star on GitHub. It helps to support the project and gives recognition to the contributors.


## Getting Started
To get started with the URL shortener service, follow the installation instructions provided in the Installation section above. Once the service is up and running, you can begin using the API endpoints to shorten URLs, track statistics, and manage your shortened links.

### Problem

The primary problem with `long URLs` is their lack of user-friendliness and practicality in various contexts. Long URLs can be cumbersome to manually input, difficult to remember, and can exceed character limitations imposed by certain platforms, such as social media posts or text messages. 
> This can result in broken links, truncated URLs, or user frustration when attempting to share or access lengthy URLs in constrained environments. 

![image](https://github.com/thisisnabi/Shortener/assets/3371886/c987259d-d62f-4eec-be90-23d00c676a9a)

### Solution

To address the problem of long URLs and make them more manageable for users, a URL shortener service provides an effective solution. By condensing lengthy URLs into shorter, more concise forms, users can easily share and access links across various platforms and communication channels. 


![image](https://github.com/thisisnabi/Shortener/assets/3371886/35fce872-feaf-4f14-bc58-54f72433e7c0)


### Shortening URLs
Implement a URL shortening algorithm to generate unique, short codes or aliases for long URLs. This algorithm should produce short codes that are both compact and unlikely to collide with existing codes in the system.
![image](https://github.com/thisisnabi/Shortener/assets/3371886/9d53ddd5-b68a-4899-9843-3d3b4185de18)

```csharp
public async Task<string> GenerateShortenUrlAsync(string destinationUrl, CancellationToken cancellation)
{
    var shortenCode = GenerateCode(destinationUrl);

    var link = new Link
    {
        CreatedOn = DateTime.UtcNow,
        DestinationUrl = destinationUrl,
        ShortenCode = shortenCode
    };

    await _dbContext.Links.AddAsync(link, cancellation);
    await _dbContext.SaveChangesAsync(cancellation);

    return $"{_appSettings.BaseUrl}/{shortenCode}";
}
```






### Redirection Mechanism






## License
This project is licensed under the MIT License: [MIT License](https://opensource.org/licenses/MIT).

## Stay Connected
Feel free to raise any questions or suggestions through GitHub issues.
