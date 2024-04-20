# Stage 1: Build an Angular Docker Image
FROM node as build
WORKDIR /apps
COPY . .
RUN npm install
COPY . /apps
ARG configuration=production
RUN npm run build -- --output-path=./dist/out --configuration $configuration
# Stage 2, use the compiled app, ready for production with Nginx
FROM nginx:alpine
COPY /nginx-custom.conf /etc/nginx/conf.d/
RUN rm -rf /usr/share/nginx/html/*
COPY --from=build /apps/dist/out/browser /usr/share/nginx/html
#RUN chown nginx:nginx /usr/share/nginx/html/*