/** @type {import('next').NextConfig} */
const nextConfig = {
  // Optimize for production deployment
  output: 'standalone',

  // Enable compression
  compress: true,

  // Optimize images
  images: {
    unoptimized: true // For Elastic Beanstalk compatibility
  },

  // Configure static file serving
  assetPrefix: process.env.NODE_ENV === 'production' ? '' : '',

  // Disable x-powered-by header for security
  poweredByHeader: false,

  // Configure redirects and rewrites if needed
  async rewrites() {
    return [
      {
        source: '/output/:path*',
        destination: '/output/:path*'
      }
    ];
  }
};

export default nextConfig;
