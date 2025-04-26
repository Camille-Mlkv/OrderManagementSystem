FROM node:20 AS node
WORKDIR /app
COPY . .

ARG GOOGLE_MAPS_API_KEY

ENV GOOGLE_MAPS_API_KEY=$GOOGLE_MAPS_API_KEY

RUN npm install
RUN npm run build --prod

RUN node replace-env.js

FROM nginx:alpine
COPY --from=node /app/dist/angular-client/browser /usr/share/nginx/html