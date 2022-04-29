import Layout from "@theme/Layout";
import React from "react";

const Pricing = () => {
  return (
    <Layout title="Pricing" description="Very simple and affordable">
      <div className="bg-white dark:bg-gray-900">
        <div className="container px-6 py-8 mx-auto">
          <div className="xl:items-center xl:-mx-8 xl:flex">
            <div className="flex flex-col items-center xl:items-start xl:mx-8">
              <h1 className="text-3xl font-medium text-gray-800 capitalize lg:text-4xl dark:text-white">
                Simple, transparent pricing
              </h1>

              <div className="mt-4">
                <span className="inline-block w-40 h-1 bg-blue-500 rounded-full"></span>
                <span className="inline-block w-3 h-1 mx-1 bg-blue-500 rounded-full"></span>
                <span className="inline-block w-1 h-1 bg-blue-500 rounded-full"></span>
              </div>

              <p className="mt-4 font-medium text-gray-500 dark:text-gray-300">
                Just a single plan. No Contracts. No surprise fees.
              </p>

              <p className="mt-4 font-medium text-gray-500 dark:text-gray-300">
                Gets cheaper the more you use it.
              </p>
            </div>

            <div className="flex-1 xl:mx-8">
              <div className="mt-8 space-y-8 md:-mx-4 md:flex md:items-center md:justify-center md:space-y-0 xl:mt-0">
                <div className="max-w-sm mx-auto border rounded-lg md:mx-4 dark:border-gray-700">
                  <div className="p-6">
                    <h1 className="text-xl font-medium text-gray-700 capitalize lg:text-3xl dark:text-white">
                      Usage
                    </h1>

                    <p className="mt-4 text-gray-500 dark:text-gray-300">
                      You're only billed what you use. Easily calculate using a{" "}
                      <a
                        href={`#pricing-calculator`}
                        className="text-sm font-bold text-blue-500 dark:text-blue-400 hover:underline"
                      >
                        pricing calculator
                      </a>
                    </p>

                    <h2 className="mt-4 text-2xl font-medium text-gray-700 sm:text-4xl dark:text-gray-300">
                      &#8369;50.00
                    </h2>

                    <p className="mt-1 text-gray-500 dark:text-gray-300">
                      flat fee with up to 50 SMS and 100 emails+ onwards
                    </p>

                    <h2 className="mt-4 text-xl font-medium text-gray-700 sm:text-2xl dark:text-gray-300">
                      + exccess SMS
                      <span className="text-base font-medium"></span>
                    </h2>

                    <p className="mt-1 text-gray-500 dark:text-gray-300">
                      see table price
                    </p>

                    <div className="bg-gray-100">
                      <table className="border-collapse border border-slate-400">
                        <tbody className="flex flex-col">
                          <tr className="flex">
                            <td className="flex-1 border border-slate-300">
                              1000+ onwards
                            </td>
                            <td className="flex-1 border border-slate-300">
                              &#8369;0.9
                              <span className="text-base font-medium">
                                /SMS
                              </span>
                            </td>
                          </tr>
                          <tr className="flex">
                            <td className="flex-1 border border-slate-300">
                              5000+ onwards
                            </td>
                            <td className="flex-1 border border-slate-300">
                              &#8369;0.8
                              <span className="text-base font-medium">
                                /SMS
                              </span>
                            </td>
                          </tr>
                          <tr className="flex">
                            <td className="flex-1 border border-slate-300">
                              10000+ onwards
                            </td>
                            <td className="flex-1 border border-slate-300">
                              &#8369;0.7
                              <span className="text-base font-medium">
                                /SMS
                              </span>
                            </td>
                          </tr>
                        </tbody>
                      </table>
                    </div>

                    <h2 className="mt-4 text-xl font-medium text-gray-700 sm:text-2xl dark:text-gray-300">
                      + exccess Email
                      <span className="text-base font-medium"></span>
                    </h2>

                    <p className="mt-1 text-gray-500 dark:text-gray-300">
                      see table price
                    </p>

                    <div className="bg-gray-100">
                      <table className="border-collapse border border-slate-400">
                        <tbody className="flex flex-col">
                          <tr className="flex">
                            <td className="flex-1 border border-slate-300">
                              5000+ onwards
                            </td>
                            <td className="flex-1 border border-slate-300">
                              &#8369;0.075
                              <span className="text-base font-medium">
                                /Email
                              </span>
                            </td>
                          </tr>
                          <tr className="flex">
                            <td className="flex-1 border border-slate-300">
                              10000+ onwards
                            </td>
                            <td className="flex-1 border border-slate-300">
                              &#8369;0.06
                              <span className="text-base font-medium">
                                /Email
                              </span>
                            </td>
                          </tr>
                          <tr className="flex">
                            <td className="flex-1 border border-slate-300">
                              15000+ onwards
                            </td>
                            <td className="flex-1 border border-slate-300">
                              &#8369;0.05
                              <span className="text-base font-medium">
                                /Email
                              </span>
                            </td>
                          </tr>
                        </tbody>
                      </table>
                    </div>

                    <p className="mt-1 text-gray-500 dark:text-gray-300">
                      Billed monthly
                    </p>

                    <button className="w-full px-4 py-2 mt-6 tracking-wide text-white capitalize transition-colors duration-200 transform bg-blue-600 rounded-md hover:bg-blue-500 focus:outline-none focus:bg-blue-500 focus:ring focus:ring-blue-300 focus:ring-opacity-80">
                      Start Now
                    </button>
                  </div>

                  <hr className="border-gray-200 dark:border-gray-700" />

                  <div className="p-6">
                    <h1 className="text-lg font-medium text-gray-700 capitalize lg:text-xl dark:text-white">
                      What’s included:
                    </h1>
                    <div className="mt-8 space-y-4">
                      <div className="flex items-center">
                        <svg
                          xmlns="http://www.w3.org/2000/svg"
                          className="w-5 h-5 text-blue-500"
                          viewBox="0 0 20 20"
                          fill="currentColor"
                        >
                          <path
                            fill-rule="evenodd"
                            d="M10 18a8 8 0 100-16 8 8 0 000 16zm3.707-9.293a1 1 0 00-1.414-1.414L9 10.586 7.707 9.293a1 1 0 00-1.414 1.414l2 2a1 1 0 001.414 0l4-4z"
                            clip-rule="evenodd"
                          />
                        </svg>

                        <span className="mx-4 text-gray-700 dark:text-gray-300">
                          Reporting and Analytics
                        </span>
                      </div>

                      <div className="flex items-center">
                        <svg
                          xmlns="http://www.w3.org/2000/svg"
                          className="w-5 h-5 text-blue-500"
                          viewBox="0 0 20 20"
                          fill="currentColor"
                        >
                          <path
                            fill-rule="evenodd"
                            d="M10 18a8 8 0 100-16 8 8 0 000 16zm3.707-9.293a1 1 0 00-1.414-1.414L9 10.586 7.707 9.293a1 1 0 00-1.414 1.414l2 2a1 1 0 001.414 0l4-4z"
                            clip-rule="evenodd"
                          />
                        </svg>

                        <span className="mx-4 text-gray-700 dark:text-gray-300">
                          Own company branding
                        </span>
                      </div>

                      <div className="flex items-center">
                        <svg
                          xmlns="http://www.w3.org/2000/svg"
                          className="w-5 h-5 text-blue-500"
                          viewBox="0 0 20 20"
                          fill="currentColor"
                        >
                          <path
                            fill-rule="evenodd"
                            d="M10 18a8 8 0 100-16 8 8 0 000 16zm3.707-9.293a1 1 0 00-1.414-1.414L9 10.586 7.707 9.293a1 1 0 00-1.414 1.414l2 2a1 1 0 001.414 0l4-4z"
                            clip-rule="evenodd"
                          />
                        </svg>

                        <span className="mx-4 text-gray-700 dark:text-gray-300">
                          Downloadable logs
                        </span>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>

      <div
        id="pricing-calculator"
        className="max-w-xs mx-auto overflow-hidden bg-white border rounded-lg shadow-lg dark:bg-gray-800"
      >
        <div className="p-6">
          <h2 className="text-xl mb-4 font-semibold text-gray-800 dark:text-white">
            Pricing Calculator
          </h2>

          <div>
            <div className="w-1/2">
              <label htmlFor="range" className="font-bold text-gray-600">
                Number of SMS
              </label>
              <input
                type="range"
                name="price"
                min="1"
                max="10000"
                className="w-full h-2 bg-blue-100 appearance-none"
              />
              <label htmlFor="range" className="font-bold text-gray-600">
                Number of Emails
              </label>
              <input
                type="range"
                name="price"
                min="1"
                max="10000"
                className="w-full h-2 bg-blue-100 appearance-none"
              />
            </div>
          </div>
          <p className="mt-1 text-sm text-gray-600 dark:text-gray-400"></p>
        </div>

        <div className="bg-gray-100">
          <h3 className="text-md font-semibold text-gray-800 dark:text-white text-center">
            Transaction breakdown
          </h3>
          <p className="mt-1 text-xs text-gray-600 dark:text-gray-400">
            Customer pays via credit or debit cards. All fees are VAT-inclusive.
          </p>

          <div className="flex items-center justify-between px-4 py-2 bg-gray-900">
            <h4 className="text-lg font-bold text-white">Total Cost</h4>
            <h4 className="text-lg font-bold text-white">$129</h4>
          </div>
        </div>
      </div>
    </Layout>
  );
};

export default Pricing;
