import Layout from "@theme/Layout";
import React from "react";

const Contact = () => {
  return (
    <Layout title="Contact Us" description="Very simple and affordable">
      <div className="container px-6 py-16 mx-auto text-center">
        <div className="max-w-xl mx-auto">
          <h1 className="text-3xl font-bold text-gray-800 dark:text-white md:text-4xl">
            Contact Us
          </h1>
          <p className="mt-6 text-lg text-gray-500 dark:text-gray-300">
            Have any questions or suggestions? We'd love to hear from you.
          </p>
        </div>
      </div>
    </Layout>
  );
};

export default Contact;
