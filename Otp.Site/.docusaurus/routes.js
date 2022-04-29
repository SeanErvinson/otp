
import React from 'react';
import ComponentCreator from '@docusaurus/ComponentCreator';

export default [
  {
    path: '/contact',
    component: ComponentCreator('/contact','8c7'),
    exact: true
  },
  {
    path: '/markdown-page',
    component: ComponentCreator('/markdown-page','332'),
    exact: true
  },
  {
    path: '/pricing',
    component: ComponentCreator('/pricing','939'),
    exact: true
  },
  {
    path: '/docs',
    component: ComponentCreator('/docs','fbd'),
    routes: [
      {
        path: '/docs/documentation',
        component: ComponentCreator('/docs/documentation','f64'),
        exact: true,
        sidebar: "tutorialSidebar"
      }
    ]
  },
  {
    path: '/',
    component: ComponentCreator('/','a45'),
    exact: true
  },
  {
    path: '*',
    component: ComponentCreator('*')
  }
];
