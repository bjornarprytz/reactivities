# Reactivities ([course link](https://www.udemy.com/course/complete-guide-to-building-an-app-with-net-core-and-react/learn/))

## How to run

Start API:

```console
cd API
dotnet run
```

Start Client:

```console
cd client-app
npm start
```

Drop Database:

```console
dotnet ef database drop -p Persistence -s API
```

## Fiddle

Use [vscode HTTP client](https://github.com/Huachao/vscode-restclient) to test against API with the [http file](API/fiddle/activities.http).

## Links

- Styling: [Semantic UI](https://semantic-ui.com/)
- Request: [Axios](https://github.com/sheaivey/react-axios)
- State: [MobX](https://mobx.js.org/README.html)
- Routing: [React Router (v6)](https://reactrouter.com/docs/en/v6/getting-started/overview)
- Forms: [Formik](https://formik.org/)
- Validation: [Yup](https://github.com/jquense/yup)
- Date Format: [date-fns](https://date-fns.org/)
- Image Storage: [Cloudinary](https://cloudinary.com/documentation/dotnet_integration)
- Real-time web: [SignalR](https://www.npmjs.com/package/@microsoft/signalr)
- Single Components
  - Date UI: [Datepicker](https://reactdatepicker.com/)
  - Dropzone: [react-dropzone](https://github.com/react-dropzone/react-dropzone)
  - Cropper: [react-cropper](https://github.com/react-cropper/react-cropper)

## Extracurricular Tasks

- Dockerize
- Refactor to use minimal APIs
- Write tests
  - React components
  - Typescript
- [Next.js](https://nextjs.org/)
- [Remix](https://remix.run/)
